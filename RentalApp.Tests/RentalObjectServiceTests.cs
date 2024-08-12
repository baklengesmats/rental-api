using Moq;
using AutoMapper;
using Xunit;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Repositories;
using RentalApp.Services;

namespace RentalApp.Tests
{
    public class RentalObjectServiceTests
    {
        private readonly Mock<IRentalObjectRepository> _rentalObjectRepository;
        private readonly Mock<IMapper> __mapper;
        private readonly RentalObjectService _rentalObjectService;

        public RentalObjectServiceTests()
        {
            _rentalObjectRepository = new Mock<IRentalObjectRepository>();
            __mapper = new Mock<IMapper>();
            _rentalObjectService = new RentalObjectService(_rentalObjectRepository.Object, __mapper.Object);
        }

        [Fact]
        public async Task GetRentalObjectById_ShouldReturnCarDto_WhenRentalObjectExists()
        {
            var registrationId = "ABC123";
            var rentalObject = new Car { RegistrationId = registrationId, Type = CarType.SmallCar };
            var carDto = new CarDto { RegistrationId = registrationId, Type = CarType.SmallCar };

            _rentalObjectRepository.Setup(repo => repo.GetRentalObject(It.IsAny<string>()))
                .ReturnsAsync(rentalObject);
            __mapper.Setup(m => m.Map<CarDto>(It.IsAny<Car>()))
                .Returns(carDto);

            var result = await _rentalObjectService.GetRentalObjectById(registrationId);

            Assert.NotNull(result);
            Assert.Equal(carDto, result);
        }

        [Fact]
        public async Task GetRentalObjectById_ShouldReturnNull_WhenRentalObjectDoesNotExist()
        {
            var registrationId = "ABC123";

            _rentalObjectRepository.Setup(repo => repo.GetRentalObject(It.IsAny<string>()))
                .ReturnsAsync((Car)null);

            var result = await _rentalObjectService.GetRentalObjectById(registrationId);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateRentalObject_ShouldCallRepositoryUpdate_WhenCalled()
        {
            var carDto = new CarDto { RegistrationId = "ABC123", Type = CarType.SmallCar };

            await _rentalObjectService.UpdateRentalObject(carDto);

            _rentalObjectRepository.Verify(repo => repo.UpdateRentalObject(It.IsAny<CarDto>()), Times.Once);
        }

        [Fact]
        public async Task GetAllRentalObjects_ShouldReturnListOfCarDto()
        {
            var rentalObjects = new List<Car>
        {
            new Car { RegistrationId = "ABC123", Type = CarType.SmallCar },
            new Car { RegistrationId = "XYZ987", Type = CarType.CombiCar }
        };
            var carDtos = new List<CarDto>
        {
            new CarDto { RegistrationId = "ABC123", Type = CarType.SmallCar },
            new CarDto { RegistrationId = "XYZ987", Type = CarType.CombiCar }
        };

            _rentalObjectRepository.Setup(repo => repo.GetRentalObjects(null, null))
                .ReturnsAsync(rentalObjects);
            __mapper.Setup(m => m.Map<IEnumerable<CarDto>>(It.IsAny<IEnumerable<Car>>()))
                .Returns(carDtos);

            var result = await _rentalObjectService.GetAllRentalObjects(null);

            Assert.NotNull(result);
            Assert.Equal(carDtos, result);
        }
    }

}