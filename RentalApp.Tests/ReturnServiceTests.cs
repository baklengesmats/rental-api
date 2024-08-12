using AutoMapper;
using Moq;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Factory;
using RentalApp.Models.Price;
using RentalApp.Repositories;
using RentalApp.Services;

namespace RentalApp.Tests
{


    public class ReturnServiceTests
    {
        private readonly Mock<IReturnRepository> _returnRepository;
        private readonly Mock<IRentalService> _rentalService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IRentalObjectService> _rentalObjectService;
        private readonly ReturnService _returnService;
        private readonly IPriceFactory _realPriceFactory;

        public ReturnServiceTests()
        {
            _returnRepository = new Mock<IReturnRepository>();
            _rentalService = new Mock<IRentalService>();
            _mapper = new Mock<IMapper>();
            _rentalObjectService = new Mock<IRentalObjectService>();
            _realPriceFactory = new PriceFactory();

            _returnService = new ReturnService(
                _returnRepository.Object,
                _rentalService.Object,
                _mapper.Object,
                _rentalObjectService.Object, _realPriceFactory);
        }

        [Fact]
        public async Task GetAllReturns_ShouldReturnReturnDtos()
        {
            var returns = new List<Returning> { new Returning { BookingNumber = "ABC123" } };
            var returnDtos = new List<ReturnDto> { new ReturnDto { BookingNumber = "ABC123" } };

            _returnRepository.Setup(repo => repo.GetAllReturnsAsync()).ReturnsAsync(returns);
            _mapper.Setup(m => m.Map<IEnumerable<ReturnDto>>(It.IsAny<IEnumerable<Returning>>())).Returns(returnDtos);

            var result = await _returnService.GetAllReturns();

            Assert.NotNull(result);
            Assert.Equal(returnDtos, result);
        }
        [Fact]
        public async Task RegisterReturn_ShouldReturnFailure_WhenKmIsInvalid()
        {
            var bookNr = Guid.NewGuid();
            var returnDto = new RegisterReturnDto { BookingNumber = bookNr.ToString(), EndedKm = 900, TimeOfReturn = DateTime.UtcNow };
            var rental = new RentalResponseDto
            {
                BookingNumber = bookNr,
                TimeOfRent = DateTime.UtcNow.AddDays(-3),
                RentalObject = new CarResponseDto { Km = 1000, Type = CarType.SmallCar },
            };

            _rentalService.Setup(s => s.GetRentalAsync(returnDto.BookingNumber)).ReturnsAsync(rental);
            _returnRepository.Setup(r => r.GetReturnByIdAsync(returnDto.BookingNumber)).ReturnsAsync((Returning)null);

            var result = await _returnService.RegisterReturn(returnDto);

            Assert.False(result.Success);
            Assert.Equal("Can't have less started Km then ended Km.", result.Error);
            Assert.Equal(400, result.ErrorCode);
            _rentalService.Verify(s => s.GetRentalAsync(returnDto.BookingNumber), Times.Once);
            _returnRepository.Verify(r => r.GetReturnByIdAsync(returnDto.BookingNumber), Times.Once);
        }


        [Fact]
        public async Task RegisterReturn_ShouldReturnFailure_WhenRentalDoesNotExist()
        {
            var registerReturnDto = new RegisterReturnDto { BookingNumber = "ABC123" };

            _rentalService.Setup(service => service.GetRentalAsync(It.IsAny<string>())).ReturnsAsync((RentalResponseDto)null);

            var result = await _returnService.RegisterReturn(registerReturnDto);

            Assert.False(result.Success);
            Assert.Equal(404, result.ErrorCode);
            Assert.Equal($"Rental with booking number: {registerReturnDto.BookingNumber} not found.", result.Error);
        }

        [Fact]
        public async Task RegisterReturn_ShouldReturnFailure_WhenRentalAlreadyReturned()
        {
            var registerReturnDto = new RegisterReturnDto { BookingNumber = "ABC123" };
            var existingReturn = new Returning { BookingNumber = "ABC123" };

            _rentalService.Setup(service => service.GetRentalAsync(It.IsAny<string>())).ReturnsAsync(new RentalResponseDto());
            _returnRepository.Setup(repo => repo.GetReturnByIdAsync(It.IsAny<string>())).ReturnsAsync(existingReturn);

            var result = await _returnService.RegisterReturn(registerReturnDto);

            Assert.False(result.Success);
            Assert.Equal(400, result.ErrorCode);
            Assert.Equal($"Rental with booking nr: {registerReturnDto.BookingNumber}, has already been returned.", result.Error);
        }

        [Fact]
        public async Task RegisterReturn_ShouldReturnSuccess_WhenRentalIsSuccessfullyReturned()
        {
            var bookNr = Guid.NewGuid();
            var returnDto = new RegisterReturnDto
            {
                BookingNumber = bookNr.ToString(),
                TimeOfReturn = DateTime.UtcNow,
                EndedKm = 1500
            };

            var rental = new RentalResponseDto
            {
                BookingNumber = bookNr,
                TimeOfRent = DateTime.UtcNow.AddDays(-3),
                RentalObject = new CarResponseDto { RegistrationId = "ABC123", Km = 1000, Type = CarType.SmallCar}
            };

            var carDto = new CarDto
            {
                RegistrationId = "ABC123",
                BaseDayPrice = 100
            };


            _rentalService.Setup(s => s.GetRentalAsync(returnDto.BookingNumber)).ReturnsAsync(rental);
            _returnRepository.Setup(r => r.GetReturnByIdAsync(returnDto.BookingNumber)).ReturnsAsync((Returning)null);
            _rentalObjectService.Setup(s => s.GetRentalObjectById(carDto.RegistrationId)).ReturnsAsync(carDto);
            _mapper.Setup(m => m.Map<CarDto>(rental.RentalObject)).Returns(carDto);
            _mapper.Setup(m => m.Map<ReturnDto>(It.IsAny<Returning>())).Returns(new ReturnDto { BookingNumber = bookNr.ToString(), Price = 300 });

            var result = await _returnService.RegisterReturn(returnDto);

            Assert.True(result.Success);
            Assert.Equal(300, result.Value.Price);
            _rentalService.Verify(s => s.GetRentalAsync(returnDto.BookingNumber), Times.Once);
            _returnRepository.Verify(r => r.GetReturnByIdAsync(returnDto.BookingNumber), Times.Once);
            _rentalObjectService.Verify(s => s.UpdateRentalObject(It.IsAny<CarDto>()), Times.Once);
            _returnRepository.Verify(r => r.AddReturnAsync(It.IsAny<Returning>()), Times.Once);
            _mapper.Verify(m => m.Map<ReturnDto>(It.IsAny<Returning>()), Times.Once);
        }

        [Fact]
        public async Task RegisterReturn_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            var registerReturnDto = new RegisterReturnDto { BookingNumber = "ABC123" };

            _rentalService.Setup(service => service.GetRentalAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Database error"));

            var result = await _returnService.RegisterReturn(registerReturnDto);

            Assert.False(result.Success);
            Assert.Equal("Database error", result.Error);
            Assert.Equal(500, result.ErrorCode);
        }
    }

}