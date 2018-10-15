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
using Todolist.Service.Auth.Exceptions;
using Todolist.Service.Auth.Models;
using Todolist.Service.Auth.Services;

namespace Todolist.Service.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AuthController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<string> Login(LoginViewModel model)
        {
            return await accountService.LoginAsync(model.Name, model.Password);
        }

        public class LoginViewModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}