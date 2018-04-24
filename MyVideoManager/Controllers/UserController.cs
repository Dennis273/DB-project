using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyVideoManager.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public string UserRole { get; set; }
        public string newPassword { get; set; }
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
                var loginUser = await _userManager.FindByNameAsync(user.UserName);
                return Accepted(new UserData {
                    UserName = loginUser.UserName,
                    Email = loginUser.Email,
                    Id = loginUser.Id,
                });
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
                await _userManager.AddToRoleAsync(user, "Admin");
                _logger.LogInformation("User created a new account with password.");
                return Created("/Register", new UserData
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserRole = "Admin",
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
        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Accepted();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete("{Username}")]
        public async Task<IActionResult> DeleteMember(string Username)
        {
            User targetUser = await _userManager.FindByNameAsync(Username);
            if (targetUser == null) return BadRequest();
            var roles = await _userManager.GetRolesAsync(targetUser);
            if (roles.Contains("Member") == false)
            {
                return BadRequest();
            }
            await _userManager.DeleteAsync(targetUser);
            return Accepted();
        }
        /// <summary>
        /// Update User Info with given user data
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Info")]
        public async Task<IActionResult> UpdateUser([FromBody] UserData userData)
        {
            ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            if (userData.Email != null) user.Email = userData.Email;
            if (userData.UserName != null) user.UserName = userData.UserName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Accepted();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPatch("Password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserData userData)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, userData.Password, userData.newPassword);
            if (result.Succeeded)
            {
                return Accepted();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
