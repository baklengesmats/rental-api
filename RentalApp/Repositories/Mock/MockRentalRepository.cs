using RentalApp.Entities;

namespace RentalApp.Repositories.Mock
{
    public class MockRentalRepository : IRentalRepository
    {
        private readonly MockDataService _mockDataService;

        public MockRentalRepository(MockDataService mockDataService) {
            _mockDataService = mockDataService ?? throw new ArgumentNullException(nameof(mockDataService));
        }
        public Task AddRentalAsync(Rental rental)
        {
            rental.Id = _mockDataService.Bookings.Count() + 1;
            _mockDataService.Bookings.Add(rental);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteRental(string bookingNr)
        {
            var booking = _mockDataService.Bookings.FirstOrDefault(b => b.BookingNumber.ToString() == bookingNr);
            if (booking == null) {
                return Task.FromResult(false);
            }

            _mockDataService.Bookings.Remove(booking);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Rental>> GetAllRentalsAsync()
        {
            return Task.FromResult(_mockDataService.Bookings.Cast<Rental>());
        }

        public Task<Rental> GetRentalByIdAsync(string bookingNr)
        {
            return Task.FromResult(_mockDataService.Bookings.FirstOrDefault(b => b.BookingNumber.ToString() == bookingNr));
        }
    }
}
