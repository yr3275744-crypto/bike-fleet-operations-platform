using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        public async Task<ActionResult<IEnumerable<GetStationsResponseDto>>> GetStations(bool? isRenting)
        {
            //var result = await _stationsService.GetStations(isRenting);
            //return Ok(result);
            var stopwatch = Stopwatch.StartNew();

            var result = await _stationsService.GetStations(isRenting);

            stopwatch.Stop();

            Console.WriteLine($"GetStations took: {stopwatch.ElapsedMilliseconds} ms");

            return Ok(result);
        }

    }
}
