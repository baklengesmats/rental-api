using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentalApp.DtoModels;
using RentalApp.Services;

namespace RentalApp.Controllers
{
    [Route("api/return")]
    [ApiController]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _returnService;

        public ReturnController(IReturnService returnService)
        {
            _returnService = returnService;
        }

        [HttpGet]
        public async Task<ActionResult<RentalDto>> GetAllReturns()
        {
            var result = await _returnService.GetAllReturns();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{bookNr}", Name = "GetReturn")]
        public async Task<ActionResult<RentalDto>> GetReturn(string bookNr)
        {
            var result = await _returnService.GetReturn(bookNr);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReturnDto>> RegisterReturn(RegisterReturnDto registerReturnDto)
        {
            var result = await _returnService.RegisterReturn(registerReturnDto);
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

            return CreatedAtRoute("GetReturn",
                new
                {
                    bookNr = result.Value.BookingNumber,
                },
                result.Value);

        }
    }
}
