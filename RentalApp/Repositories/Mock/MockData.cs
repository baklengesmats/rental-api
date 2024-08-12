using RentalApp.Entities;

namespace RentalApp.Repositories.Mock
{
    public class MockData
    {
        public static List<Rental> GetMockBookings()
        {
            var customers = GetMockCustomers();
            var cars = GetMockCars();

            return new List<Rental>
        {
            new Rental
            {
                Id = 1,
                BookingNumber = new Guid("bcd07424-99e9-4980-bd1e-5200a884a1f2"),
                HashPersonalNr = customers[0].HashedPersonNumber,
                Customer = customers[0],
                RentalObjectId = cars[0].Id,
                RentalObject = cars[0],
                TimeOfRent = new DateTime(2024, 8, 10, 14, 30, 0)
            },
            new Rental
            {
                Id = 2,
                BookingNumber = new Guid("d583b63d-4f76-4175-ad41-0eaa6574c703"),
                HashPersonalNr = customers[1].HashedPersonNumber,
                Customer = customers[1],
                RentalObjectId = cars[1].Id,
                RentalObject = cars[1],
                TimeOfRent = new DateTime(2024, 8, 12, 10, 0, 0)
            }
        };
        }

        public static List<Car> GetMockCars()
        {
            return new List<Car>
        {
            new Car
            {
                Id = 1,
                RegistrationId = "ABC123",
                IsAvailable = false,
                Type = CarType.SmallCar,
                Km = 50000,
                BaseDayPrice = 30.0,
                BaseKmPrice = 0.2
            },
            new Car
            {
                Id = 2,
                RegistrationId = "XYZ789",
                IsAvailable = false,
                Type = CarType.SmallCar,
                Km = 75000,
                BaseDayPrice = 50.0,
                BaseKmPrice = 0.25
            },
            new Car
            {
                Id = 3,
                RegistrationId = "LMN456",
                IsAvailable = true,
                Type = CarType.CombiCar,
                Km = 30000,
                BaseDayPrice = 70.0,
                BaseKmPrice = 0.30
            },
            new Car
            {
                Id = 3,
                RegistrationId = "LMN788",
                IsAvailable = true,
                Type = CarType.Truck,
                Km = 10000,
                BaseDayPrice = 100.0,
                BaseKmPrice = 0.5
            },
            new Car
            {
                Id = 4,
                RegistrationId = "GHF900",
                IsAvailable = true,
                Type = CarType.CombiCar,
                Km = 5000,
                BaseDayPrice = 70.0,
                BaseKmPrice = 0.3
            },
            new Car
            {
                Id = 5,
                RegistrationId = "SSN888",
                IsAvailable = true,
                Type = CarType.Truck,
                Km = 5400,
                BaseDayPrice = 100.0,
                BaseKmPrice = 0.5
            },
             new Car
            {
                Id = 6,
                RegistrationId = "SGT886",
                IsAvailable = true,
                Type = CarType.SmallCar,
                Km = 1000,
                BaseDayPrice = 50.0,
                BaseKmPrice = 0
            }
        };
        }

        public static List<Customer> GetMockCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    PersonNumber = "1234567890",
                    HashedPersonNumber = "HASHED1234567890"
                },
                new Customer
                {
                    Id = 2,
                    PersonNumber = "0987654321",
                    HashedPersonNumber = "HASHED0987654321"
                },
                new Customer
                {
                    Id = 3,
                    PersonNumber = "1122334455",
                    HashedPersonNumber = "HASHED1122334455"
                }
            };

        }

        public static List<Returning> GetMockReturns()
        {
            var rentals = GetMockBookings();
            var cars = GetMockCars();
            return new List<Returning>
            {
                new Returning
                {
                    Id = 1,
                    BookingNumber = rentals[0].BookingNumber.ToString(),
                    Price = 500,
                    RegistrationId = cars[0].RegistrationId,
                    TimeOfReturn = new DateTime(2024, 8, 11, 14, 30, 0)
                }
            };
        }


    }
}
