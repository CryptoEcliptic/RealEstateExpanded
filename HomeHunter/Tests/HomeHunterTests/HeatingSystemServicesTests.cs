﻿using AutoMapper;
using HomeHunter.Data;
using HomeHunter.Domain;
using HomeHunter.Models.ViewModels.HeatingSystem;
using HomeHunter.Services;
using HomeHunter.Services.Models.HeatingSystem;
using HomeHunterTests.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHunterTests
{
    [TestFixture]
    public class HeatingSystemServicesTests
    {
        private const string ResultCountMismatchMessage = "Expected test result should be 3";
        private const string ExpectedTrueResultMessage = "Expected result should return true, but it is false!";

        private List<HeatingSystem> TestData = new List<HeatingSystem>
        {
            new HeatingSystem { Name = "ТЕЦ", Id = 1},
            new HeatingSystem { Name = "Ток", Id = 2},
            new HeatingSystem { Name = "Локално отопление", Id = 3},
        };
        private HomeHunterDbContext context;
        public HeatingSystemServicesTests()
        {
            context = InMemoryDatabase.GetDbContext();
            this.SeedData();
        }

        [Test]
        public async Task GetAllHeatingSystemTypesCountShouldReturnThree()
        {
            var mapper = this.GetMapper();

            var heatingSystemService = new HeatingSystemServices(context);
            var heatingSystemTypes = await heatingSystemService.GetAllHeatingSystemsAsync();
            var heatingSystemViewModel = mapper.Map<List<HeatingSystemViewModel>>(heatingSystemTypes);

            int numberOftypes = heatingSystemViewModel.Count();
            var expectedResultCount = 3;
            Assert.That(numberOftypes, Is.EqualTo(expectedResultCount), ResultCountMismatchMessage);
        }


        [Theory]
        [TestCase("ТЕЦ", "ТЕЦ")]
        [TestCase(null, null)]
        public async Task GetHeatingSystemTypeByNameShouldReturnTrue(string type, string expectedResult)
        {

            var heatingSystemService = new HeatingSystemServices(context);
            var heatingSystemType = await heatingSystemService.GetHeatingSystemAsync(type);

            string actualResult = heatingSystemType != null ? heatingSystemType.Name : null;

            Assert.That(actualResult, Is.EqualTo(expectedResult), ExpectedTrueResultMessage);
        }

        private void SeedData()
        {
            context.HeatingSystems.AddRange(TestData);
            context.SaveChanges();
        }

        private IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(x =>
            {
                x.CreateMap<HeatingSystemServiceModel, HeatingSystemViewModel>();
            });

            var mapper = new Mapper(configuration);
            return mapper;
        }
    }
}
