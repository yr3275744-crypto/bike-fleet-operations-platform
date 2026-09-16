using Microsoft.EntityFrameworkCore;
using ProcessingService.Data;
using ProcessingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingService.Servicese
{
    public class StationInformationHandler
    {
        private readonly ApplicationDbContext _dbContext;
        public StationInformationHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(StationInformation information)
        {
            if (await _dbContext.StationInformations
                .FirstOrDefaultAsync(s => s.StationId == information.StationId)
                == null)
            {
                _dbContext.StationInformations
                    .Add(information);
            }
            var result = await _dbContext.SaveChangesAsync();
            return (result > 0);
        }
        public async Task<bool> UpdateAsync(StationInformation stationInformation)
        {
            var exist = await _dbContext.StationInformations
                .FirstOrDefaultAsync(s => s.StationId == stationInformation.StationId);

            if (exist == null)
            {
                return false;
            }

            exist.Name = stationInformation.Name;
            exist.ShortName = stationInformation.ShortName;
            exist.Longitude = stationInformation.Longitude;
            exist.Latitude = stationInformation.Latitude;
            exist.RegionId = stationInformation.RegionId;
            exist.Capacity = stationInformation.Capacity;

            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
