using DataAccessLibrary.DataAccess;
using ToDoList.Dtos;
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
            _authorizationManager.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new()
            {
                Username = userDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            if (await _userRepository.TryCreateNewUserAsync(user) is false)
            {
                return BadRequest("Username already taken");
            }

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
