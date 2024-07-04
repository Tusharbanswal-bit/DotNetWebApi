using webAPIApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webAPIApp.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace webAPIApp.Controllers {

    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase {
        private readonly UserService _user;
        private readonly IOptions<JwtSettings> _config;
        private readonly PasswordHasher<object> _passwordHasher;

        public LoginController(UserService user, IOptions<JwtSettings> config) {
            _config = config;
            _user = user;
            _passwordHasher = new PasswordHasher<object>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User user) {
            var userDetails = await _user.GetAsync(user.Username);
            var passwordVerfication = _passwordHasher.VerifyHashedPassword(null, userDetails.Password, user.Password);
            if (userDetails == null && passwordVerfication == PasswordVerificationResult.Success) {
                return Unauthorized();
            }
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userDetails.Id),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Sectoken = new JwtSecurityToken(_config.Value.Issuer, _config.Value.Issuer, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return Ok(token);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signin(User user) {
            user.Password = _passwordHasher.HashPassword(null, user.Password);
            await _user.CreateAsync(user);
            return CreatedAtAction(nameof(Login), new {id = user.Username}, user);
        }
    }
}