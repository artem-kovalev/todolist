using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Todolist.Service.Auth.Extensions
{
    public static class IdentityResultExtensions
    {
        public static void ThrowException(this IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(c => $"{c.Code} - {c.Description}");
                var message = string.Join(Environment.NewLine, errors);
                throw new Exception(message);
            }
        }
    }
}