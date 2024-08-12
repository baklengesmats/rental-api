using AutoMapper;
using Moq;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Models;
using RentalApp.Models.Price;
using RentalApp.Repositories;
using RentalApp.Repositories.Mock;
using RentalApp.Services;

namespace RentalApp.Tests
{
    public class RentalServiceTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IRentalRepository> _rentalRepository;
        private readonly Mock<IRentalObjectRepository> _rentalObjectRepository;
        private readonly RentalService _rentalService;
        private RegisterRentalDto registerDto;
        public RentalServiceTests() {
            _mapper = new Mock<IMapper>();
            _rentalObjectRepository = new Mock<IRentalObjectRepository>();
            _rentalRepository = new Mock<IRentalRepository>();
            _rentalService = new RentalService(_rentalRepository.Object, _rentalObjectRepository.Object, _mapper.Object);

            registerDto = new RegisterRentalDto() { PersonNumber = "1232324", RegistrationId = "ABC123", TimeOfRent = new DateTime(2024, 8, 11, 14, 30, 0) };
        }

        [Theory]
        [InlineData("XYZ123")]
        [InlineData("DDD111")]
        public async Task AddRentalAsync_ShouldReturnFailure_WhenRentalObjectNotFound(string registrationId)
        {
            registerDto.RegistrationId = registrationId;
            _rentalObjectRepository.Setup(repo => repo.GetRentalObject(It.IsAny<string>()))
                .ReturnsAsync((Car)null);

            var result = await _rentalService.RegisterRental(registerDto);

            Assert.False(result.Success);
            Assert.Equal($"Rental Object: {registrationId} not found or not available.", result.Error);
        }

        [Theory]
        [InlineData("XYZ123")]
        [InlineData("DDD111")]
        public async Task AddRentalAsync_ShouldReturnFailure_WhenRentalObjectIsNotAvailable(string registrationId)
        {
            var car = new Car() { IsAvailable = false, RegistrationId = registrationId };
            registerDto.RegistrationId = registrationId;

            _rentalObjectRepository.Setup(repo => repo.GetRentalObject(It.IsAny<string>()))
                .ReturnsAsync(car);
            var result = await _rentalService.RegisterRental(registerDto);

            Assert.False(result.Success);
            Assert.Equal(404, result.ErrorCode);
            Assert.Equal($"Rental Object: {registrationId} not found or not available.", result.Error);
        }


        [Fact]
        public async Task RegisterRental_ShouldReturnSuccess_WhenRentalIsSuccessfullyCreated()
        {
            var rentalObject = new Car { RegistrationId = "XYZ123", IsAvailable = true };
            var registerRentalDto = new RegisterRentalDto { RegistrationId = "XYZ123", PersonNumber = "1234567890" };
            var rental = new Rental { BookingNumber = Guid.NewGuid(), RentalObject = rentalObject, Customer = new Customer { PersonNumber = "1234567890" } };
            var rentalResponseDto = new RentalResponseDto { BookingNumber = rental.BookingNumber, PersonNumber = "1234567890" };

            _rentalObjectRepository.Setup(repo => repo.GetRentalObject(It.IsAny<string>()))
                .ReturnsAsync(rentalObject);
            _mapper.Setup(m => m.Map<Rental>(It.IsAny<RegisterRentalDto>())).Returns(rental);
            _mapper.Setup(m => m.Map<RentalResponseDto>(It.IsAny<Rental>())).Returns(rentalResponseDto);
            _rentalRepository.Setup(repo => repo.AddRentalAsync(It.IsAny<Rental>())).Returns(Task.CompletedTask);

            var result = await _rentalService.RegisterRental(registerRentalDto);

            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.Equal(rentalResponseDto, result.Value);
            Assert.False(rentalObject.IsAvailable);
            _rentalRepository.Verify(repo => repo.AddRentalAsync(It.IsAny<Rental>()), Times.Once);
        }

        [Fact]
        public async Task RegisterRental_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            var registerRentalDto = new RegisterRentalDto { RegistrationId = "XYZ123", PersonNumber = "1234567890" };
            _rentalObjectRepository.Setup(repo => repo.GetRentalObject(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _rentalService.RegisterRental(registerRentalDto);

            Assert.False(result.Success);
            Assert.Equal("Database error", result.Error);
            Assert.Equal(500,result.ErrorCode);
        }


        [Fact]
        public async Task GetRentalAsync_ShouldReturnRentalResponseDto_WhenRentalExists()
        {
            var bookingNumber = Guid.NewGuid();
            var rental = new Rental { BookingNumber = bookingNumber, Customer = new Customer { PersonNumber = "1234567890" } };
            var rentalResponseDto = new RentalResponseDto { BookingNumber = bookingNumber, PersonNumber = "1234567890" };

            _rentalRepository.Setup(repo => repo.GetRentalByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(rental);
            _mapper.Setup(m => m.Map<RentalResponseDto>(It.IsAny<Rental>())).Returns(rentalResponseDto);

            var result = await _rentalService.GetRentalAsync(bookingNumber.ToString());

            Assert.NotNull(result);
            Assert.Equal(rentalResponseDto, result);
        }

        [Fact]
        public async Task GetRentalAsync_ShouldReturnNull_WhenRentalDoesNotExist()
        {
            var bookingNumber = "ABC123";

            _rentalRepository.Setup(repo => repo.GetRentalByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Rental)null);

            var result = await _rentalService.GetRentalAsync(bookingNumber);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetRentalsAsync_ShouldReturnRentalResponseDtoList_WhenRentalsExist()
        {
            var rentals = new List<Rental>
        {
            new Rental { BookingNumber = Guid.NewGuid(), Customer = new Customer { PersonNumber = "1234567890" }},
            new Rental { BookingNumber = Guid.NewGuid(), Customer = new Customer { PersonNumber = "9876543210" }}
        };
            var rentalResponseDtos = new List<RentalResponseDto>
        {
            new RentalResponseDto { BookingNumber = rentals[0].BookingNumber, PersonNumber = "1234567890" },
            new RentalResponseDto { BookingNumber = rentals[1].BookingNumber, PersonNumber = "9876543210" }
        };

            _rentalRepository.Setup(repo => repo.GetAllRentalsAsync())
                .ReturnsAsync(rentals);
            _mapper.Setup(m => m.Map<IEnumerable<RentalResponseDto>>(It.IsAny<IEnumerable<Rental>>()))
                .Returns(rentalResponseDtos);

            var result = await _rentalService.GetRentalsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(rentalResponseDtos, result);
        }

        [Fact]
        public async Task GetRentalsAsync_ShouldReturnEmptyList_WhenNoRentalsExist()
        {
            var rentals = new List<Rental>();
            _rentalRepository.Setup(repo => repo.GetAllRentalsAsync())
                .ReturnsAsync(rentals);
            _mapper.Setup(m => m.Map<IEnumerable<RentalResponseDto>>(It.IsAny<IEnumerable<Rental>>()))
                .Returns(new List<RentalResponseDto>());

            var result = await _rentalService.GetRentalsAsync();
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}