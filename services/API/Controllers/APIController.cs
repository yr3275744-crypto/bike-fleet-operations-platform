using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api")]
    public class APIController : ControllerBase
    {
        private readonly IStationsService _stationsService;
        public APIController(IStationsService stationsService)
        {
            _stationsService = stationsService;
        }
        [HttpGet("stations")]
        public async Task<ActionResult<IEnumerable<GetStationsResponseDto>>> GetStations()
        {
            var result = await _stationsService.GetStations();
            return Ok(result);
        }

    }
}
