using AutoMapper;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Factory;
using RentalApp.Models;
using RentalApp.Models.Price;
using RentalApp.Repositories;
using RentalApp.Utils;

namespace RentalApp.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IRentalService _rentalService;
        private readonly IMapper _mapper;
        private readonly IRentalObjectService _rentalObjectService;
        private readonly IPriceFactory _priceFactory;

        public ReturnService(IReturnRepository returnRepository, IRentalService rentalService,
            IMapper mapper, IRentalObjectService rentalObjectService, IPriceFactory priceFactory)
        {
            _returnRepository = returnRepository ?? throw new ArgumentNullException(nameof(returnRepository));
            _rentalService = rentalService ?? throw new ArgumentNullException(nameof(rentalService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _rentalObjectService = rentalObjectService ?? throw new ArgumentNullException(nameof(rentalObjectService));
            _priceFactory = priceFactory;
        }

        public async Task<IEnumerable<ReturnDto>> GetAllReturns()
        {
            var returnList = await _returnRepository.GetAllReturnsAsync();
            return _mapper.Map<IEnumerable<ReturnDto>>(returnList);
        }

        public async Task<ReturnDto> GetReturn(string bookNr)
        {
            return _mapper.Map<ReturnDto>(await _returnRepository.GetReturnByIdAsync(bookNr));
        }

        public async Task<Result<ReturnDto>> RegisterReturn(RegisterReturnDto returnDto)
        {
            try
            {
                var rental = await _rentalService.GetRentalAsync(returnDto.BookingNumber);
                if (rental == null)
                {
                    return Result<ReturnDto>.FailureResult($"Rental with booking number: {returnDto.BookingNumber} not found.", 404);
                }

                if (await IsReturnAlreadyRegistered(returnDto.BookingNumber))
                {
                    return Result<ReturnDto>.FailureResult($"Rental with booking nr: {returnDto.BookingNumber}, has already been returned.", 400);
                }

                var rentedDays = CalculateRentedDays(rental.TimeOfRent, returnDto.TimeOfReturn);
                if (rentedDays < 0)
                {
                    return Result<ReturnDto>.FailureResult($"Invalid input: RentDate: {rental.TimeOfRent} ReturnDate: {returnDto.TimeOfReturn}", 400);
                }

                if (!IsKmValid(rental.RentalObject.Km, returnDto.EndedKm))
                {
                    return Result<ReturnDto>.FailureResult("Can't have less started Km then ended Km.", 400);
                }
                var usedKm = returnDto.EndedKm - rental.RentalObject.Km;
                var price = CalculatePrice(rental, usedKm, rentedDays);
                await UpdateRentalObject(returnDto.EndedKm, rental.RentalObject.RegistrationId);

                var newReturn = CreateNewReturn(rental, returnDto, price);
                await _returnRepository.AddReturnAsync(newReturn);

                var resultDto = _mapper.Map<ReturnDto>(newReturn);
                return Result<ReturnDto>.SuccessResult(resultDto);
            }
            catch (Exception e)
            {
                return Result<ReturnDto>.FailureResult(e.Message, null);
            }

        }

        private async Task<bool> IsReturnAlreadyRegistered(string bookingNumber)
        {
            var existingReturn = await _returnRepository.GetReturnByIdAsync(bookingNumber);
            return existingReturn != null;
        }

        private int CalculateRentedDays(DateTime timeOfRent, DateTime timeOfReturn)
        {
            var totalHours = (timeOfReturn - timeOfRent).TotalHours;
            var days = Math.Ceiling(totalHours / 24);

            return days > 0 ? (int)days : (int)(days == 0 ? 1 : -1);
        }

        private bool IsKmValid(int startKm, int endedKm)
        {
            return endedKm > startKm;
        }

        private double CalculatePrice(RentalResponseDto rental, int usedKm, int rentedDays)
        {
            var carDto = _mapper.Map<CarDto>(rental.RentalObject);
            var priceStrategy = _priceFactory.CreatePriceStrategy(carDto, usedKm);
            var rentalCar = new RentalCar(carDto.RegistrationId, carDto.BaseDayPrice, rentedDays, rental.RentalObject.Km+usedKm, priceStrategy);
            var result = rentalCar.Price();
            return result;
        }

        private async Task UpdateRentalObject(int endedKm, string registrationId)
        {
            var car = await _rentalObjectService.GetRentalObjectById(registrationId);
            car.Km = endedKm;
            car.IsAvailable = true;
            await _rentalObjectService.UpdateRentalObject(car);
        }

        private Returning CreateNewReturn(RentalResponseDto rental, RegisterReturnDto returnDto, double price)
        {
            return new Returning
            {
                BookingNumber = returnDto.BookingNumber,
                Price = price,
                RegistrationId = rental.RentalObject.RegistrationId,
                TimeOfReturn = returnDto.TimeOfReturn,
                TimeOfRent = rental.TimeOfRent,
            };
        }

    }
}
