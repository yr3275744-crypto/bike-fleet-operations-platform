using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api")]
    public class APIController : ControllerBase
    {
        private readonly IStationInformationService _informationService;
        private readonly IStationStatusService _statusService;
        public APIController(IStationInformationService informationService,
            IStationStatusService statusService)
        {
            _informationService = informationService;
            _statusService = statusService;
        }
    }
}
