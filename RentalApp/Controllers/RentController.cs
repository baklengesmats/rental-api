using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalApp.DtoModels;
using RentalApp.Repositories;
using RentalApp.Services;
using System.Net.Sockets;

namespace RentalApp.Controllers
{
    [Route("api/rentals")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentController(IRentalService rentalService, IRentalObjectService rentalObjectService)
        {
            _rentalService = rentalService;
        }

        [HttpGet("{bookNr}", Name = "GetRental")]
        public async Task<ActionResult<RentalResponseDto>> GetRental(string bookNr)
        {
            var result = await _rentalService.GetRentalAsync(bookNr);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalResponseDto>>> GetAllRentals()
        {
            var result = await _rentalService.GetRentalsAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<RentalResponseDto>> RegisterRental(RegisterRentalDto rentalCreation)
        {
            var result = await _rentalService.RegisterRental(rentalCreation);
            if (!result.Success)
            {
                switch (result.ErrorCode)
                {
                    case 400:
                        return BadRequest(result.Error);
                    case 404: 
                        return NotFound(result.Error);
                    case 409:
                        return Conflict(result.Error);
                    default: 
                        return StatusCode(500, new { Message = result.Error });
                        
                }
            }

            return CreatedAtRoute("GetRental",
                new
                {
                    bookNr = result.Value.BookingNumber,
                },
                result.Value);

        }



    }
}
