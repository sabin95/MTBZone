using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using IdentityAPI.Commands;
using IdentityAPI.Data;
using IdentityAPI.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAPI.Repository
{
    public class IdentityService : IIdentityService
    {
        private readonly IConfiguration _config;
        private readonly IdentityContext _context;

        public IdentityService(IdentityContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public Task<string> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(_config["JwtSettings:Issuer"],
            _config["JwtSettings:Audience"],
            claims,
            expires: DateTime.Now.AddHours(4),
            signingCredentials: credentials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<User> RegisterUser(UserRegisterCommand userRegisterCommand, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRegisterCommand.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var newUser = new User
            {
                Name = userRegisterCommand.Name,
                Email = userRegisterCommand.Email,
                Role = UserRoles.User
            };

            newUser.SetPassword(password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
    }
}
