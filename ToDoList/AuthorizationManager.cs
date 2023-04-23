using DataAccessLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ToDoList
{
    public class AuthorizationManager
    {
        private readonly IConfiguration _configuration;

        public AuthorizationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a password hash and salt.
        /// </summary>
        /// <param name="password">Plain text password given by the user.</param>
        /// <param name="passwordHash">Generated password hash.</param>
        /// <param name="passwordSalt">Generated password salt.</param>
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Checks if the password's hash and salt match with the repository.
        /// </summary>
        /// <param name="password">Plain text password given by the user.</param>
        /// <param name="passwordHash">Password hash from the repository.</param>
        /// <param name="passwordSalt">Password salt from the repository.</param>
        /// <returns></returns>
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// Creates a JSON Web Token for the logged in user.
        /// </summary>
        /// <param name="user">Logged in user.</param>
        /// <returns>JSON Web Token with user's Name claim.</returns>
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration.GetSection("Jwt:Issuer").Value,
                _configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
