using InventoryApp.Contracts.Dtos;
using InventoryApp.Contracts.Options;
using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models.Users.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryApp.Services
{
    public class UserAuthenticationService
    {
        private readonly IUserProvider _userProvider;

        private SecretOption SecretOptions { get; }

        public UserAuthenticationService(IUserProvider adminProvider, IOptions<SecretOption> secretOptions)
        {
            SecretOptions = secretOptions.Value;
            _userProvider = adminProvider;
        }
        public async Task<(string, int?)> Authenticate(string login, string password)
        {
            try {
                var admin = await _userProvider.GetByEmail(login);

                if (!BCrypt.Net.BCrypt.Verify(password, admin.Password)) {
                    throw new ArgumentException("Incorrect password");
                }

                return (GenerateJwtToken(admin.Email, admin.Role), (int) admin.Role);
            }
            catch (ArgumentException e)
            {
                return (null, null);
            }
        }
        private string GenerateJwtToken(string login, UserRole role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.UserData, login),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
        private UserClaimsDto DecryptToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            if (tokenS?.Claims is List<Claim> claims)
            {
                return new UserClaimsDto
                {
                    Email = claims[0].Value,
                    Role = (UserRole)((Enum.TryParse(typeof(UserRole), claims[1].Value, true, out var role)
                        ? role
                        : throw new ArgumentException()) ?? throw new ArgumentException())
                };
            }

            throw new ArgumentException();
        }
        public async Task<User> GetUserByHeaders(string[] headers)
        {
            var token = headers[0].Replace("Bearer ", "");
            var result = await _userProvider.GetByEmail(DecryptToken(token).Email);

            return result;
        }

    }
}
