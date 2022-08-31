using DataAccessLibrary;
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

        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _authorizationManager = new(_configuration);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto userDto)
        {
            if (_userRepository.GetUser(userDto.Username) is not null)
            {
                return ValidationProblem("Username is taken");
            }

            _authorizationManager.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new()
            {
                Username = userDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _userRepository.CreateNewUser(user);
            await _userRepository.SaveListAsync();
            
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto userDto)
        {
            var user = _userRepository.GetUser(userDto.Username);

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
