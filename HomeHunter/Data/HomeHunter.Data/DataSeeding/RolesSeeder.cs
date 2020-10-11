using HomeHunter.Common;
using HomeHunter.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHunter.Data.DataSeeding
{
    public class RolesSeeder : ISeeder
    {
        private readonly IConfiguration configuration;

        public RolesSeeder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SeedAsync(HomeHunterDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var usermanager = serviceProvider.GetRequiredService<UserManager<HomeHunterUser>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName); 
            await SeedRoleAsync(roleManager, GlobalConstants.UserRoleName);
            await SeedUserAdminRole(usermanager);
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private async Task SeedUserAdminRole(UserManager<HomeHunterUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                await CreateAdminUserAsync(userManager);
            }
            else
            {
                var user = await userManager.FindByEmailAsync("writetorado@abv.bg");
                user.PasswordHash = this.configuration["AdminUserPasswordHash"];
                await userManager.UpdateAsync(user);
            } 
        }

        private static async Task CreateAdminUserAsync(UserManager<HomeHunterUser> userManager)
        {
            var user = new HomeHunterUser
            {
                UserName = "writetorado@abv.bg",
                Email = "writetorado@abv.bg",
                FirstName = "AdminFirstName",
                LastName = "AdminLastName",
                EmailConfirmed = true,
                CreatedOn = DateTime.Now,
            };

            var password = "123456";

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
