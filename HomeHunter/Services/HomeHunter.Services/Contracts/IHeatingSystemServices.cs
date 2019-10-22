﻿using HomeHunter.Domain;
using HomeHunter.Services.Models.HeatingSystem;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Services.Contracts
{
    public interface IHeatingSystemServices
    {
        Task<IQueryable<HeatingSystemServiceModel>> GetAllHeatingSystemsAsync();

        Task<HeatingSystem> GetHeatingSystemAsync(string systemName);
    }
}
