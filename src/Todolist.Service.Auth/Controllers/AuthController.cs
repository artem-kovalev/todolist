using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Todolist.Service.Auth.Configuration;
using Todolist.Service.Auth.Models;

namespace Todolist.Service.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        private readonly AppSettings settings;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, IOptions<AppSettings> settings)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.settings = settings.Value;
        }

        [HttpPost]
        public async Task<object> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Name, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.Name);
                return await GenerateJwtToken(user);
            }
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        private async Task<object> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(settings.JwtSecret);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(1));

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires : expires,
                signingCredentials : creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginViewModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}