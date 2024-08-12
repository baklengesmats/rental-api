using RentalApp.Entities;

namespace RentalApp.Repositories
{
    public interface IRentalRepository
    {
        Task AddRentalAsync(Rental rental);
        Task<Rental> GetRentalByIdAsync(string bookingNr);
        Task<IEnumerable<Rental>> GetAllRentalsAsync();
        Task<bool> DeleteRental(string bookingNr);
    }
}
