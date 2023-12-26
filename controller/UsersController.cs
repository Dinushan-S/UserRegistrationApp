using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserRegistrationApp.Data;
using UserRegistrationApp.Models;
using BCrypt.Net;

namespace UserRegistrationApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authenticationService.Register2(model);

            if (result)
            {
                return Ok();
            }
            else
            {
                // foreach (var error in result.Errors)
                // {
                //     ModelState.AddModelError(string.Empty, error.Description);
                // }
                ModelState.AddModelError(string.Empty, "Registration failed.");
                return BadRequest(ModelState);
            }
        }
    }
}