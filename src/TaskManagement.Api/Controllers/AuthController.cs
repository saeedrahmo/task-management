using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<Core.Models.ApplicationUser> _signInManager;
        private readonly UserManager<Core.Models.ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(SignInManager<Core.Models.ApplicationUser> signInManager, UserManager<Core.Models.ApplicationUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await GenerateTokenAsync(model.UserName, model.Password);

                if (token != null)
                {
                    // Token generated successfully
                    return Ok(new { Token = token });
                }
                else
                {
                    // Invalid username or password
                    return BadRequest("Invalid username or password");
                }
            }

            // Model validation failed
            return BadRequest(ModelState);
        }

        private async Task<string> GenerateTokenAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                // User not found
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                // Invalid password
                return null;
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        // Add additional claims as needed
    };

            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);

            var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,            
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
            signingCredentials: signingCredentials
            );


            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }
    }
}
