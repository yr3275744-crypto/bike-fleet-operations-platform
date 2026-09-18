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
    }
}
