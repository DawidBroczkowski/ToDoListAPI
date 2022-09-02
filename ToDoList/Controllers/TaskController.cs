using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataAccess.DbData;
using DataAccessLibrary.Dtos;
using DataAccessLibrary.DataAccess.JsonData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private DataAccessLibrary.Models.User _currentUser;
        private IUserRepository _userRepository;

        public TaskController(UsersContext db, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userRepository.SetContext(db);
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<FullTaskDto>> GetTasksAsync()
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            var tasks = _currentUser.TaskManager.GetTasks();
            if (tasks is not null)
            {
                var result = tasks.Select(t => t.AsDto());
                return result;
            }
            return null;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<FullTaskDto>> GetTaskAsync(Guid id)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            var task = _currentUser.TaskManager.GetTask(id);

            if (task is null)
            {
                return NotFound();
            }

            return task.AsDto();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateTaskAsync(CreateNewTaskDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            _currentUser.TaskManager.CreateNewTask(taskDto.Name, taskDto.Description);
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut("Start/{id}")]
        public async Task<ActionResult> StartTaskAsync(Guid id)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryStartTask(id) is false)
            {
                return NotFound();
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut("Complete/{id}")]
        public async Task<ActionResult> CompleteTaskAsync(Guid id)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryCompleteTask(id) is false)
            {
                return NotFound();
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut("Update/Name")]
        public async Task<ActionResult> UpdateTaskNameAsync(UpdateTaskNameDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryUpdateTaskName(taskDto.Id, taskDto.Name) is false)
            {
                return NotFound();
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut("Update/Description")]
        public async Task<ActionResult> UpdateTaskNameAsync(UpdateTaskDescriptionDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryUpdateTaskDescription(taskDto.Id, taskDto.Description) is false)
            {
                return NotFound();
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut("Update/Status")]
        public async Task<ActionResult> UpdateTaskStatusAsync(UpdateTaskStatusDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryUpdateTaskStatus(taskDto.Id, taskDto.Status) is false)
            {
                return NotFound();
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskAsync(Guid id)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryDeleteTask(id) is false)
            {
                return NotFound();
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }
    }
}