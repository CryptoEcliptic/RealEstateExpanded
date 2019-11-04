using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace HomeHunter.Data.DataSeeding
{
    public interface ISeeder
    {
       Task SeedAsync(HomeHunterDbContext dbContext, IServiceProvider serviceProvider);
    }
}
