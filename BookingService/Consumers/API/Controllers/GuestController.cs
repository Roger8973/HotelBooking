using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Guest.Responses;
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

            if (response.ErrorCode == Application.ErrosCodes.NOT_FOUND)
                return BadRequest(response);

            _logger.LogError("Response with unknown ErroCode Returned", response);

            return BadRequest(500);
        }
    }
}
