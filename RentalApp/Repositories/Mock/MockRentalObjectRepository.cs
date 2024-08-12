using RentalApp.DtoModels;
using RentalApp.Entities;

namespace RentalApp.Repositories.Mock
{
    public class MockRentalObjectRepository : IRentalObjectRepository
    {
        private readonly MockDataService _mockDataService;

        public MockRentalObjectRepository(MockDataService mockDataService) {
            _mockDataService = mockDataService ?? throw new ArgumentNullException(nameof(mockDataService));
        }

        public async Task<Car> GetRentalById(string registrationId)
        {
            return _mockDataService.Cars.FirstOrDefault(b => b.RegistrationId == registrationId);
        }

        public Task<IEnumerable<Car>> GetRentalObjects(CarType? type, bool? isAvailable)
        {
            var carfilter = _mockDataService.Cars;
            if(type != null)
            {
                carfilter = carfilter.Where(b => b.Type == type).ToList();
            }
            if (isAvailable != null)
            {
                carfilter = carfilter.Where(b => b.IsAvailable == isAvailable).ToList();
            }

            return Task.FromResult<IEnumerable<Car>>(carfilter);
        }

        public async Task<Car> GetRentalObject(string registraionId)
        {
            return _mockDataService.Cars.FirstOrDefault(c => c.RegistrationId == registraionId);
        }

        public Task UpdateRentalObject(CarDto rentalObject)
        {
            var result = _mockDataService.Cars.FirstOrDefault(c => c.RegistrationId == rentalObject.RegistrationId);
            result.Km = rentalObject.Km;
            result.IsAvailable = rentalObject.IsAvailable;
            return Task.CompletedTask;
        }


    }
}
