using RentalApp.Entities;

namespace RentalApp.Repositories
{
    public interface IReturnRepository
    {
        Task AddReturnAsync(Returning rental);
        Task<Returning> GetReturnByIdAsync(string bookingNr);
        Task<IEnumerable<Returning>> GetAllReturnsAsync();
 
    }
}
