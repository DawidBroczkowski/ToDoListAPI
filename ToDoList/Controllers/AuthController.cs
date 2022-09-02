using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Dtos;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly AuthorizationManager _authorizationManager;

        public AuthController(IConfiguration configuration, IUserRepository userRepository, UsersContext db)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userRepository.SetContext(db);
            _authorizationManager = new(_configuration);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto userDto)
        {
            if (_userRepository.GetUserAsync(userDto.Username) is not null)
            {
                return ValidationProblem("Username is taken");
            }
            if (userDto.Password.Length < 6 || userDto.Password.Length > 30)
            {
                return ValidationProblem("Password must be between 6-30 characters long");
            }
            if (userDto.Username.Length < 4 || userDto.Username.Length > 20)
            {
                return ValidationProblem("Password must be between 4-20 characters long");
            }

            _authorizationManager.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new()
            {
                Username = userDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.CreateNewUserAsync(user);
            await _userRepository.SaveListAsync();
            
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto userDto)
        {
            var user = await _userRepository.GetUserAsync(userDto.Username);

            if (user is null)
            {
                return BadRequest("User not found.");
            }

            if(!_authorizationManager.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }

            string token = _authorizationManager.CreateToken(user);
            return Ok(token);
        }
    }
}
