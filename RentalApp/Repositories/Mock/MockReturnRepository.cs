using RentalApp.Entities;

namespace RentalApp.Repositories.Mock
{
    public class MockReturnRepository : IReturnRepository
    {
        private readonly MockDataService _mockDataService;

        public MockReturnRepository(MockDataService mockDataService) {
            _mockDataService = mockDataService ?? throw new ArgumentNullException(nameof(mockDataService));
        }

        public async Task AddReturnAsync(Returning rental)
        {
            rental.Id = _mockDataService.Returning.Count + 1;
            _mockDataService.Returning.Add(rental);
        }

        public Task<IEnumerable<Returning>> GetAllReturnsAsync()
        {
            return Task.FromResult(_mockDataService.Returning.Cast<Returning>());
        }

        public Task<Returning> GetReturnByIdAsync(string bookingNr)
        {
           return Task.FromResult(_mockDataService.Returning.FirstOrDefault(r => r.BookingNumber == bookingNr));
        }
    }
}
