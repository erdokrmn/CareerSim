using CareerSim.Models;
using Microsoft.AspNetCore.Identity;

namespace CareerSim.Data
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Admin rolü yoksa oluştur
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            // Admin kullanıcı bilgileri
            var adminEmail = "admin@example.com";
            var adminUserName = "admin";
            var adminPassword = "Admin1234*"; // Yayına geçmeden önce değiştir!

            // Admin kullanıcıyı kontrol et ve yoksa oluştur
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var adminUser = new User
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
