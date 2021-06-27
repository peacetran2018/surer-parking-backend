using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using parking_backend.Configs;
using parking_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace parking_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("userprofile")]
        public async Task<Object> GetUserProfile(string email)
        {
            var existingUser = await this._userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new
                {
                    existingUser.FirstName,
                    existingUser.LastName,
                    existingUser.Email,
                    existingUser.PhoneNumber
                };
            }
            return BadRequest();
        }
    }
}
