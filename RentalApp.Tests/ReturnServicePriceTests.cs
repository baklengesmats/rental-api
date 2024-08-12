using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Factory;
using RentalApp.Repositories;
using RentalApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Tests
{
    public class ReturnServicePriceTests
    {
        private readonly Mock<IReturnRepository> _returnRepository;
        private readonly Mock<IRentalService> _rentalService;
        private readonly IMapper _mapper;
        private readonly Mock<IRentalObjectService> _rentalObjectService;
        private readonly ReturnService _returnService;
        private readonly IPriceFactory _realPriceFactory;

        public ReturnServicePriceTests()
        {
            var services = new ServiceCollection();

            // Configure AutoMapper to scan the current domain's assemblies
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Build the service provider to resolve services
            var serviceProvider = services.BuildServiceProvider();

            // Resolve the IMapper service
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _returnRepository = new Mock<IReturnRepository>();
            _rentalService = new Mock<IRentalService>();
            _rentalObjectService = new Mock<IRentalObjectService>();
            _realPriceFactory = new PriceFactory();

            _returnService = new ReturnService(
                _returnRepository.Object,
                _rentalService.Object,
                _mapper,
                _rentalObjectService.Object, _realPriceFactory);
        }

        [Theory]
        [InlineData(CarType.SmallCar, 3, 50, 0.3, 150)]
        [InlineData(CarType.CombiCar, 3, 70, 0.5, 298)]
        [InlineData(CarType.Truck, 5, 100, 0.7, 802.5)]//days * baseDayPrice * 1.5 + Km * BaseKmPrice* 1.5;
        public async Task RegisterReturn_ShouldCalculateCorrectPrice_ForDifferentCarTypes(CarType carType, int rentedDays, double baseDayPrice, double baseKmPrice, double expectedPrice)
        {
            var bookNr = Guid.NewGuid();
            var registerReturnDto = new RegisterReturnDto
            {
                BookingNumber = bookNr.ToString(),
                TimeOfReturn = DateTime.UtcNow,
                EndedKm = 1050
            };

            var rental = new RentalResponseDto
            {
                BookingNumber = bookNr,
                TimeOfRent = DateTime.UtcNow.AddDays(-rentedDays),
                RentalObject = new CarResponseDto { RegistrationId = "ABC123", Km = 1000, Type = carType, BaseDayPrice = baseDayPrice, BaseKmPrice = baseKmPrice }
            };

            var carDto = new CarDto
            {
                RegistrationId = "ABC123",
                BaseDayPrice = baseDayPrice,
                BaseKmPrice = baseKmPrice,
                Type = carType,
            };

            var newReturn = new Returning
            {
                BookingNumber = bookNr.ToString(),
                RegistrationId = carDto.RegistrationId,
                TimeOfRent = rental.TimeOfRent,
                TimeOfReturn = registerReturnDto.TimeOfReturn,
            };

            var returnDto = new ReturnDto
            {
                BookingNumber = registerReturnDto.BookingNumber,
                RegistrationId = carDto.RegistrationId,
                TimeOfRent = rental.TimeOfRent,
                TimeOfReturn = registerReturnDto.TimeOfReturn
            };


            _rentalService.Setup(s => s.GetRentalAsync(registerReturnDto.BookingNumber)).ReturnsAsync(rental);
            _returnRepository.Setup(r => r.GetReturnByIdAsync(registerReturnDto.BookingNumber)).ReturnsAsync((Returning)null);
            _rentalObjectService.Setup(s => s.GetRentalObjectById(carDto.RegistrationId)).ReturnsAsync(carDto);
            var result = await _returnService.RegisterReturn(registerReturnDto);

            Assert.True(result.Success); 
            Assert.Equal(expectedPrice, result.Value.Price);
        }
    }
}
