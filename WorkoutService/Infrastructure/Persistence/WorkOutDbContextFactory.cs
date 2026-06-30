using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkoutService.Infrastructure.Persistence
{
    public class WorkOutDbContextFactory : IDesignTimeDbContextFactory<WorkoutDbContext>
    {
        public WorkoutDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorkoutDbContext>();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new WorkoutDbContext(optionsBuilder.Options);
        }
    }
}
