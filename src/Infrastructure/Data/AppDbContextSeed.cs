using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                context.Database.Migrate();

                await roleManager.CreateAsync(new IdentityRole("Admin"));

                var dafaultAdminUser = new AppUser
                {
                    UserName = "DefaultUser",
                    Email = "default@user.com",

                    EmailConfirmed = true,
                    AvatarUrl = "https://test/image.png",

                    FirstName = "AdminName",
                    SecondName = "AdminSecondName",
                    LastName = "AdminLastName"
                };
                await userManager.CreateAsync(dafaultAdminUser, "@Default123");

                dafaultAdminUser = await userManager.FindByNameAsync("DefaultUser");
                await userManager.AddToRoleAsync(dafaultAdminUser, "Admin");

                var dafaultUser = new AppUser
                {
                    UserName = "DefaultAlex",
                    Email = "alex@user.com",

                    EmailConfirmed = true,
                    AvatarUrl = "https://test/alex.png",

                    FirstName = "Alexandr",
                    SecondName = "Ivanovich",
                    LastName = "Stepanov"
                };
                await userManager.CreateAsync(dafaultUser, "@Default123");

                if (!await context.ShopItems.AnyAsync())
                {
                    await context.ShopItems.AddRangeAsync(
                        GetDefaultItems());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<AppDbContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, userManager, roleManager, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<ShopItem> GetDefaultItems()
        {
            return new List<ShopItem>()
            {
                new ShopItem("Носки обычные", "Шерстяные носки", "https://test/images/items/2.png", 99),
                new ShopItem("Шорты Nike", "Шорты Nike отлично подойдут для занятий спортом", "https://test/images/items/3.png", 650),
                new ShopItem("Шорты Adidas", "Шорты Adidas обычно используются для летней прогулки", "https://test/images/items/4.png", 530),
                new ShopItem("Штаны", "Штаны с начесом, зимние", "https://test/images/items/5.png", 2290),
                new ShopItem("Джемпер", "Джемпер красный", "https://test/images/items/6.png", 3110),
                new ShopItem("Шляпа", "Шляпа декоративная", "https://test/images/items/7.png", 2700),
                new ShopItem("Шапка зимняя", "Шапка шерстяная, зимняя, до -30 градусов", "https://test/images/items/8.png", 780),
                new ShopItem("Джинсы", "Джинсы синие", "https://test/images/items/9.png", 2450),
                new ShopItem("Джинсы Color", "Джинсы Color, черные, дизайнерские", "https://test/images/items/10.png", 5650),
                new ShopItem("Футболка", "Футболка, позволяет коже дышать", "https://test/images/items/11.png", 1050),
            };
        }
    }
}
