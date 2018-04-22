using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyVideoManager.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVideoManager.Controllers
{
    public class UserData
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
        public string Email { get; set; }
    }
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserData user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, true, false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in");
                return NoContent();
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserData userData)
        {
            if (userData == null) return BadRequest();
            var user = new User { UserName = userData.UserName, Email = userData.Email };
            var result = await _userManager.CreateAsync(user, userData.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                return Created("/Register", new UserData
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                });
            }
            else
            {
                List<Object> errors = new List<Object>();
                foreach (IdentityError identityError in result.Errors)
                {
                    errors.Add(identityError);
                }
                return BadRequest(errors);
            }
        }

    }
}
