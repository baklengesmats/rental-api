using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Utils;

namespace RentalApp.Services
{
    public interface IRentalService
    {
        Task<IEnumerable<RentalResponseDto>> GetRentalsAsync();
        Task<RentalResponseDto> GetRentalAsync(string bookingNr);
        Task<Result<RentalResponseDto>> RegisterRental(RegisterRentalDto registerRental);

    }
}
