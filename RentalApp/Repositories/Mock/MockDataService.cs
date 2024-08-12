using RentalApp.Entities;

namespace RentalApp.Repositories.Mock
{
    public class MockDataService
    {
        public List<Rental> Bookings { get; set; }
        public List<Car> Cars { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Returning> Returning { get; set; }

        public MockDataService()
        {
            Bookings = MockData.GetMockBookings();
            Cars = MockData.GetMockCars();
            Customers = MockData.GetMockCustomers();
            Returning = MockData.GetMockReturns();
        }
    }
}
