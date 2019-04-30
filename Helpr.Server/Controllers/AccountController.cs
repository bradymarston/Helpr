using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpr.Server.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrayerJournal.Core.Dtos;
using ShadySoft.ControllerErrorHelpers.Extensions.Controller;

namespace Helpr.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync("Email-" + model.Email, model.Password, true, true);
            
            if (result.Succeeded)
            {
                return Ok();
            }

            return this.SignInFailure(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registration)
        {
            if (await _userManager.FindByNameAsync(registration.Email) != null)
                return this.IdentityFailure("EmailInUse", "Email already in use.");

            var user = new ApplicationUser
            {
                UserName = "Email-" + registration.Email,
                Email = registration.Email
            };

            var result = await _userManager.CreateAsync(user, registration.Password);
            if (result.Succeeded)
            {
//                await SendEmailTokenAsync(user);

                await _signInManager.SignInAsync(user, true);          
                return Ok();
            }

            return this.IdentityFailure(result);
        }
    }
}