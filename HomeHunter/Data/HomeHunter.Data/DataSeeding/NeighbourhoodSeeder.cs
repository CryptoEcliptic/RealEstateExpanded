using HomeHunter.Common;
using HomeHunter.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Data.DataSeeding
{
    public class NeighbourhoodSeeder : ISeeder
    {

        public async Task SeedAsync(HomeHunterDbContext dbContext, IServiceProvider serviceProvider)
        {
            await SeedNeighbourhoodsAsync(GlobalConstants.ImotBgSofiaDistricts, dbContext);
        }

        private static async Task SeedNeighbourhoodsAsync(List<string> neighbourhoods, HomeHunterDbContext dbContext)
        {
            var neighbourhoodsFromDb = dbContext.Neighbourhoods.ToList();
            var citiesFromDb = dbContext.Cities.ToList();
            var createdNeighbourhoods = new List<Neighbourhood>();

            foreach (var row in neighbourhoods)
            {
                var splitRow = row.Split(new string[] { ", " }, StringSplitOptions.None);
                var cityName = splitRow[0].Split(' ')[1];
                var name = splitRow[1];

                var cityId = citiesFromDb.FirstOrDefault(x => x.Name == cityName).Id;

                if (!neighbourhoodsFromDb.Any(x => x.Name == name))
                {
                    createdNeighbourhoods.Add(new Neighbourhood
                    {
                        Name = name,
                        CreatedOn = DateTime.Now,
                        CityId = cityId,
                    });
                }
            }

            await dbContext.Neighbourhoods.AddRangeAsync(createdNeighbourhoods);
            await dbContext.SaveChangesAsync();
        }
    }
}
