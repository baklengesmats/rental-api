using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalApp.DtoModels;
using RentalApp.Services;

namespace RentalApp.Controllers
{
    [Route("api/rentals/cars")]
    [ApiController]
    public class RentalObjectController : ControllerBase
    {
        private readonly IRentalObjectService _rentalObjectService;

        public RentalObjectController(IRentalObjectService rentalObjectService)
        {
            _rentalObjectService = rentalObjectService;
        }

        [HttpGet]
        public async Task<ActionResult<CarDto>> GetAllRental()
        {
            var result = await _rentalObjectService.GetAllRentalObjects(null);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
