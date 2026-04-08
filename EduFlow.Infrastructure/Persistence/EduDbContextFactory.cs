using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EduFlow.Infrastructure.Persistence.Context
{
    public class EduDbContextFactory : IDesignTimeDbContextFactory<EduDbContext>
    {
        public EduDbContext CreateDbContext(string[] args)
        {
           
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<EduDbContext>();
            var connectionString = configuration.GetConnectionString("EduFlow");

            builder.UseSqlServer(connectionString);

            return new EduDbContext(builder.Options);
        }
    }
}