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
    public class VehicleTypesHandler
    {
        private readonly ApplicationDbContext _dbContext;
        public VehicleTypesHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(VehicleType vehicleType)
        {
            if (await _dbContext.VehicleTypes
                .FirstOrDefaultAsync(v => v.VehicleTypeId == vehicleType.VehicleTypeId)
                == null)
            {
                _dbContext.VehicleTypes
                .Add(vehicleType);
            }
            var result = await _dbContext.SaveChangesAsync();
            return (result > 0);
        }
        public async Task<bool> UpdateAsync(VehicleType vehicleType)
        {
            var exist = await _dbContext.VehicleTypes
                .FirstOrDefaultAsync(v => v.VehicleTypeId == vehicleType.VehicleTypeId);
            if (exist == null)
            {
                return false;
            }
            exist.FormFactor = vehicleType.FormFactor;
            exist.PropulsionType = vehicleType.PropulsionType;
            exist.MaxRangeMeters = vehicleType.MaxRangeMeters;
            var result = await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
