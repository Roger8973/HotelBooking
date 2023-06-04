using Application;
using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestController : Controller
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IGuestManager _guestManager;

        public GuestController(ILogger<GuestController> logger, IGuestManager guestManager)
        {
            _logger = logger;
            _guestManager = guestManager;
        }

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> Post(GuestDTO guest)
        {
            var request = new CreateGuestRequest
            {
                Data = guest,
            };

            var response = await _guestManager.CreateGuest(request);

            if (response.Success) 
                return Created("", request.Data);

            //Can be moved from controller to method
            switch (response.ErrorCode)
            {
                case ErrosCodes.NOT_FOUND:
                    return BadRequest(response);

                case ErrosCodes.INVALID_PERSON_ID: 
                    return BadRequest(response);

                case ErrosCodes.MISSING_REQUIRED_INFORMATION:
                    return BadRequest(response);

                case ErrosCodes.INVALID_EMAIL:
                    return BadRequest(response);

                case ErrosCodes.COULD_NOT_STORE_DATA:
                    return BadRequest(response);
            }

            _logger.LogError("Response with unknown ErroCode Returned", response);

            return BadRequest(500);
        }
    }
}
