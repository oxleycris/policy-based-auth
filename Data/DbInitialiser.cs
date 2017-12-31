using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PBA.Models;

namespace PBA.Data
{
    // http://www.locktar.nl/programming/net-core/seed-database-users-roles-dotnet-core-2-0-ef/
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            context.Database.EnsureCreated();

            // Create non admin roles, and check for any new roles that need to be seeded.
            await AddRoles(roleManager, logger);

            // Look for existing admin user.
            if (context.Users.Any(x => x.Email == "oxley.cris@gmail.com"))
            {
                return;
            }

            await CreateDefaultUserAndRoleForApplication(userManager, roleManager, logger);
        }

        private static async Task AddRoles(RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            var rolesList = new List<IdentityRole>
            {
                // TODO: Add IsEnabled or IsActive flag, extend to new Role class.
                new IdentityRole("Developer"),
                new IdentityRole("Tester"),
                new IdentityRole("Architect"),
                new IdentityRole("Manager"),
                new IdentityRole("Marketing")
            };

            foreach (var role in rolesList)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    var ir = await roleManager.CreateAsync(role);

                    if (ir.Succeeded)
                    {
                        logger.LogDebug($"Created the role `{role.Name}` successfully");
                    }
                    else
                    {
                        var exception = new ApplicationException($"Role `{role.Name}` cannot be created");
                        logger.LogError(exception, GetIdentityErrorsInCommaSeperatedList(ir));

                        throw exception;
                    }
                }
            }
        }

        private static async Task CreateDefaultUserAndRoleForApplication(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            const string adminRole = "Admin";
            const string email = "oxley.cris@gmail.com";

            await CreateDefaultAdminRole(roleManager, logger, adminRole);

            var user = await CreateDefaultUser(userManager, logger, email);

            await SetPasswordForDefaultUser(userManager, logger, email, user);
            await AddDefaultRoleToDefaultUser(userManager, logger, email, adminRole, user);
        }

        private static async Task CreateDefaultAdminRole(RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger, string adminRole)
        {
            logger.LogInformation($"Create the role `{adminRole}` for application");

            var ir = await roleManager.CreateAsync(new IdentityRole(adminRole));

            if (ir.Succeeded)
            {
                logger.LogDebug($"Created the role `{adminRole}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Default role `{adminRole}` cannot be created");
                logger.LogError(exception, GetIdentityErrorsInCommaSeperatedList(ir));

                throw exception;
            }
        }

        private static async Task<ApplicationUser> CreateDefaultUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email)
        {
            logger.LogInformation($"Create default user with email `{email}` for application");

            var user = new ApplicationUser(email, "Admin", "Admin", new DateTime(1980, 04, 14));
            var ir = await userManager.CreateAsync(user);

            if (ir.Succeeded)
            {
                logger.LogDebug($"Created default user `{email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Default user `{email}` cannot be created");
                logger.LogError(exception, GetIdentityErrorsInCommaSeperatedList(ir));
                throw exception;
            }

            var createdUser = await userManager.FindByEmailAsync(email);

            return createdUser;
        }

        private static async Task SetPasswordForDefaultUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email, ApplicationUser user)
        {
            logger.LogInformation($"Set password for default user `{email}`");

            const string password = "P@55w0rd";
            var ir = await userManager.AddPasswordAsync(user, password);

            if (ir.Succeeded)
            {
                logger.LogTrace($"Set password `{password}` for default user `{email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Password for the user `{email}` cannot be set");
                logger.LogError(exception, GetIdentityErrorsInCommaSeperatedList(ir));

                throw exception;
            }
        }

        private static async Task AddDefaultRoleToDefaultUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email, string adminRole, ApplicationUser user)
        {
            logger.LogInformation($"Add default user `{email}` to role '{adminRole}'");

            var ir = await userManager.AddToRoleAsync(user, adminRole);

            if (ir.Succeeded)
            {
                logger.LogDebug($"Added the role '{adminRole}' to default user `{email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"The role `{adminRole}` cannot be set for the user `{email}`");
                logger.LogError(exception, GetIdentityErrorsInCommaSeperatedList(ir));

                throw exception;
            }
        }

        private static string GetIdentityErrorsInCommaSeperatedList(IdentityResult ir)
        {
            string errors = null;

            foreach (var identityError in ir.Errors)
            {
                errors += identityError.Description;
                errors += ", ";
            }

            return errors;
        }
    }
}
