using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Todolist.Service.Auth.Extensions;
using Todolist.Service.Auth.Models;

namespace Todolist.Service.Auth.Configuration
{
    public static class IdentityDataInitializer
    {
        public static void SeedUsers(this UserManager<User> manager)
        {
            if (manager.FindByNameAsync("admin").Result == null)
            {
                var admin = new User("admin", "admin@localhost");
                admin.EmailConfirmed = true;

                var result = manager.CreateAsync(admin, "admin123").Result;
                if (result.Succeeded)
                {
                    manager.AddToRoleAsync(admin, RoleConstants.Administrator).Wait();
                }
                else
                {
                    result.ThrowException();
                }
            }
        }

        public static void SeedRoles(this RoleManager<Role> manager)
        {
            if (!manager.RoleExistsAsync(RoleConstants.Administrator).Result)
            {
                var role = new Role(RoleConstants.Administrator);
                var result = manager.CreateAsync(role).Result;
                if (!result.Succeeded)
                {
                    result.ThrowException();
                }
            }
        }
    }
}