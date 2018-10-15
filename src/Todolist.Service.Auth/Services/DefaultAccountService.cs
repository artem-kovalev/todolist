using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Todolist.Service.Auth.Configuration;
using Todolist.Service.Auth.Exceptions;
using Todolist.Service.Auth.Models;

namespace Todolist.Service.Auth.Services
{
    public class DefaultAccountService : IAccountService
    {
        private readonly SignInManager<User> signInManager;
        private readonly AppSettings settings;
        private readonly ILogger<DefaultAccountService> logger;
        public DefaultAccountService(IOptions<AppSettings> settings, ILogger<DefaultAccountService> logger, SignInManager<User> signInManager)
        {
            this.settings = settings.Value;
            this.logger = logger;
            this.signInManager = signInManager;
        }

        public async Task<string> LoginAsync(string name, string password)
        {
            var result = await signInManager.PasswordSignInAsync(name, password, false, false);
            if (result.Succeeded)
            {
                logger.LogDebug($"Login successed by login: {name}");

                var user = await signInManager.UserManager.FindByNameAsync(name);
                return await GenerateJwtToken(user);
            }

            logger.LogInformation($"Failed to login by: {name}");
            throw new LoginFailException(name);
        }

        private Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(settings.JwtSecret);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddSeconds(settings.JwtExpiresSecconds);

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires : expires,
                signingCredentials : creds
            );

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(result);
        }
    }
}