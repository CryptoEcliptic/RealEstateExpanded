﻿using HomeHunter.Data;
using HomeHunter.Domain;
using HomeHunter.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Services
{
    public class AddressServices : IAddressServices
    {
        private const string AddressNotFoundMessage = "No such address in the database!";
        private readonly HomeHunterDbContext context;

        public AddressServices(HomeHunterDbContext context)
        {
            this.context = context;
        }

        public async Task<Address> CreateAddressAsync(City city, string description, Village village, Neighbourhood neighbourhood)
        {
            var address = new Address
            {
                City = city,
                Description = description,
                Village = village,
                Neighbourhood = neighbourhood
            };

            this.context.Addresses.Add(address);
            return address;
        }


        public async Task<Address> EditAddressAsync(int addressId, City city, string description, Village village, Neighbourhood neighbourhood)
        {
            var address = this.context.Addresses
                .Include(x => x.Village)
                .FirstOrDefault(x => x.Id == addressId)
                ;

            if (address == null)
            {
                throw new ArgumentNullException(AddressNotFoundMessage);
            }

            address.City = city;
            address.CityId = city == null ? address.CityId = null : address.CityId = city.Id;
            address.Village = village;
            address.Neighbourhood = neighbourhood;

            if (city == null || city.Name != "София")
            {
                address.Neighbourhood = null;
                address.NeighbourhoodId = null;
            }

            address.Description = description;
            address.ModifiedOn = DateTime.Now;

            this.context.Update(address);
            await this.context.SaveChangesAsync();

            return address;
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            var address = await this.context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

            if (address == null)
            {
                return false;
            }

            address.IsDeleted = true;
            address.DeletedOn = DateTime.Now;

            try
            {
                this.context.Update(address);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

    }
}
