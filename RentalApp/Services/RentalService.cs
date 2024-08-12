using AutoMapper;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Repositories;
using RentalApp.Utils;

namespace RentalApp.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IRentalObjectRepository _rentalCarRepository;
        private readonly IMapper _mapper;

        public RentalService(IRentalRepository rentalRepository, IRentalObjectRepository rentalCarRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _rentalCarRepository = rentalCarRepository ?? throw new ArgumentNullException(nameof(rentalCarRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<Result<RentalResponseDto>> RegisterRental(RegisterRentalDto registerRental)
        {
            try
            {
                var rentalObject = await GetAvailableRentalObject(registerRental.RegistrationId);
                if (rentalObject == null)
                {
                    return Result<RentalResponseDto>.FailureResult($"Rental Object: {registerRental.RegistrationId} not found or not available.", 404);
                }

                var newRental = CreateRental(registerRental, rentalObject);

                await _rentalRepository.AddRentalAsync(newRental);

                return Result<RentalResponseDto>.SuccessResult(_mapper.Map<RentalResponseDto>(newRental));

            }catch (Exception ex)
            {
                return Result<RentalResponseDto>.FailureResult(ex.Message, null);
            }
        }

        public async Task<RentalResponseDto> GetRentalAsync(string bookingNr)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(bookingNr);
            return _mapper.Map<RentalResponseDto>(rental);
        }

        public async Task<IEnumerable<RentalResponseDto>> GetRentalsAsync()
        {
            var rentals=await _rentalRepository.GetAllRentalsAsync();
            return _mapper.Map<IEnumerable<RentalResponseDto>>(rentals); 
        }

        private async Task<Car> GetAvailableRentalObject(string registrationId)
        {
            var rentalObject = await _rentalCarRepository.GetRentalObject(registrationId);
            if (rentalObject == null || !rentalObject.IsAvailable)
            {
                return null;
            }
            return rentalObject;
        }

        private Rental CreateRental(RegisterRentalDto registerRental, Car rentalObject)
        {
            rentalObject.IsAvailable = false;

            var customer = new Customer();
            customer.PersonNumber = registerRental.PersonNumber;
            customer.HashedPersonNumber = HashHelper.ComputeSha256Hash(customer.PersonNumber);

            var newRental = _mapper.Map<Rental>(registerRental);
            newRental.RentalObject = rentalObject;
            newRental.BookingNumber = Guid.NewGuid();
            newRental.Customer = customer;

            return newRental;
        }
    }
}
