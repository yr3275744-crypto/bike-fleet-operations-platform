using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProcessingService.Data;
using ProcessingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingService.Servicese
{
    public class ContextCreatingFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        //private readonly ConfigStrings _configStrings;
        //public ContextCreatingFactory(ConfigStrings configStrings)
        //{
        //    _configStrings = configStrings;
        //}
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            DotNetEnv.Env.Load();
            var connectionString =
            Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "MYSQL_CONNECTION_STRING not found.");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
