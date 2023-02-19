using DTIS.Shared.Models;

namespace DTIS.WebApi.Data;

public static class DataSeeder
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>();

            context!.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                context.AddRange(new List<Role>()
                {
                    new Role
                    {
                        Name = "Administrator",
                    },

                    new Role
                    {
                        Name = "TechnicalWorker"
                    },

                    new Role
                    {
                        Name = "RepairServiceEmployee"
                    },
                    new Role
                    {
                        Name = "None"
                    }
                });

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var adminRole = context.Roles.FirstOrDefault(x => x.Name == "Administrator");

                context.Add( 
                    new User 
                    {
                        Email = "admin@admin.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("administrator"),
                        Role = adminRole,
                        IsActivated = true                        
                    });

                context.SaveChanges();
            }
        }
    }
}
