using DataAccessLibrary.DataAccess;
using ToDoList.Dtos;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataAccessLibrary.DataAccess.DbData;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository _repository;
        private readonly AuthorizationManager _authorizationManager;

        public AuthController(IConfiguration configuration, IRepository userRepository, ListContext db)
        {
            _configuration = configuration;
            _repository = userRepository;
            _repository.SetContext(db);
            _authorizationManager = new(_configuration);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto userDto)
        {
            if (await _repository.GetUserAsync(userDto.Username)! is not null)
            {
                return BadRequest("Username already taken");
            }
            _authorizationManager.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            _repository.CreateNewUser(userDto.Username, passwordHash, passwordSalt);
            await _repository.SaveListAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto userDto)
        {
            var user = await _repository.GetUserAsync(userDto.Username)!;

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
