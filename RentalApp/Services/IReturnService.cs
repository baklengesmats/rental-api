using Microsoft.AspNetCore.Http.HttpResults;
using RentalApp.DtoModels;
using RentalApp.Entities;
using RentalApp.Utils;

namespace RentalApp.Services
{
    public interface IReturnService
    {
        Task<Result<ReturnDto>> RegisterReturn(RegisterReturnDto returnDto);
        Task<ReturnDto> GetReturn(string bookNr);
        Task<IEnumerable<ReturnDto>> GetAllReturns();
    }
}
