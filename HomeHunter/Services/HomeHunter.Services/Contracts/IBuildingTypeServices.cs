﻿using HomeHunter.Domain;
using HomeHunter.Services.Models.BuildingType;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Services.Contracts
{
    public interface IBuildingTypeServices
    {
        Task<IQueryable<BuildingTypeServiceModel>> GetAllBuildingTypesAsync();

        Task<BuildingType> GetBuildingTypeAsync(string type);
    }
}
