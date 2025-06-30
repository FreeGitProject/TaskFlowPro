using Microsoft.AspNetCore.Identity;
using TaskFlowPro.Api.Models;
using Task = TaskFlowPro.Api.Models.Task;
using TaskStatus = TaskFlowPro.Api.Models.TaskStatus;

namespace TaskFlowPro.Api.Data
{
    public static class SeedData
    {
        public static async System.Threading.Tasks.Task Initialize(AppDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Create roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminEmail = "admin@taskflow.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create regular user
            var userEmail = "user@taskflow.com";
            var regularUser = await userManager.FindByEmailAsync(userEmail);
            if (regularUser == null)
            {
                regularUser = new User
                {
                    FirstName = "Regular",
                    LastName = "User",
                    Email = userEmail,
                    UserName = userEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(regularUser, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }

            // Seed sample tasks if none exist
            if (!context.Tasks.Any())
            {
                context.Tasks.AddRange(
                    new Task
                    {
                        Title = "Implement Authentication",
                        Description = "Set up JWT authentication for the API",
                        CreatedById = adminUser.Id,
                        AssignedToId = adminUser.Id,
                        Status = TaskStatus.Completed,
                        Priority = TaskPriority.High,
                        DueDate = DateTime.UtcNow.AddDays(-1)
                    },
                    new Task
                    {
                        Title = "Create Task Service",
                        Description = "Implement CRUD operations for tasks",
                        CreatedById = adminUser.Id,
                        AssignedToId = regularUser.Id,
                        Status = TaskStatus.InProgress,
                        Priority = TaskPriority.Medium,
                        DueDate = DateTime.UtcNow.AddDays(3)
                    },
                    new Task
                    {
                        Title = "Setup SignalR",
                        Description = "Configure real-time updates for tasks",
                        CreatedById = adminUser.Id,
                        AssignedToId = regularUser.Id,
                        Status = TaskStatus.New,
                        Priority = TaskPriority.Low,
                        DueDate = DateTime.UtcNow.AddDays(7)
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
