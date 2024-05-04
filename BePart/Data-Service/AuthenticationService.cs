using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedData;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BePart
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginUser user);
        Task<LoginResponse> RefreshToken(RefreshTokenModel model);
        Task<bool> RegisterUser(LoginUser user);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly CatDbContext _context;
        private readonly IConfiguration _config;

        public AuthenticationService(UserManager<ExtendedIdentityUser> userManager, IConfiguration config, CatDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var identityUser = new ExtendedIdentityUser
            {
                UserName = user.UserName,
                Email = user.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            return result.Succeeded;
        }

        public async Task<LoginResponse> Login(LoginUser user)
        {
            var response = new LoginResponse();
            var identityUser = await _userManager.FindByEmailAsync(user.UserName);

            if (identityUser is null || (await _userManager.CheckPasswordAsync(identityUser, user.Password)) == false)
            {
                return response;
            }

            response.IsLogedIn = true;
            response.JwtToken = GenerateTokenString(identityUser.Email);
            response.RefreshToken = GenerateRefreshTokenString();

            identityUser.RefreshToken = response.RefreshToken;
            identityUser.RefreshTokenExpiry = DateTime.Now.AddHours(12);
            await _userManager.UpdateAsync(identityUser);

            return response;
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenModel model)
        {
            var response = new LoginResponse();
            var user = _context.ExtendedUsers.SingleOrDefault(x => x.RefreshToken == model.RefreshToken);
            if (user is null)
                return response;

            var identityUser = await _userManager.FindByEmailAsync(user.Email);

            if (identityUser is null || identityUser.RefreshToken != model.RefreshToken || identityUser.RefreshTokenExpiry < DateTime.Now)
                return response;

            response.IsLogedIn = true;
            response.JwtToken = GenerateTokenString(identityUser.Email);
            response.RefreshToken = GenerateRefreshTokenString();

            identityUser.RefreshToken = response.RefreshToken;
            identityUser.RefreshTokenExpiry = DateTime.Now.AddHours(8);
            await _userManager.UpdateAsync(identityUser);

            return response;
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateTokenString(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role,"Admin"),
            };

            var staticKey = _config.GetSection("Jwt:Key").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(staticKey));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signingCred
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
