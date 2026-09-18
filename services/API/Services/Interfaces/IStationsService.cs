using API.Models;

namespace API.Services.Interfaces
{
    public interface IStationsService
    {
        Task<IEnumerable<GetStationsResponseDto>> GetStations();
    }
}
