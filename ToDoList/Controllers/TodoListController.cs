using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataAccess.DbData;
using ToDoList.Dtos;
using DataAccessLibrary.DataAccess.JsonData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using DataAccessLibrary.Models;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoListController : Controller
    {
        private DataAccessLibrary.Models.User _currentUser;
        private IUserRepository _userRepository;

        public TodoListController(UsersContext db, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userRepository.SetContext(db);
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<TodoListDto>> GetTodoListsAsync()
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            var todoLists = _currentUser.TaskManager.GetTodoLists();
            return todoLists.Select(t => t.AsDto());
        }

        [HttpGet("{listId}")]
        [Authorize]
        public async Task<ActionResult<TodoListDto>> GetTodoListAsync(Guid listId)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            var todoList = _currentUser.TaskManager.GetTodoList(listId);
            if (todoList is null)
            {
                return BadRequest();
            }
            return Ok(todoList.AsDto());
        }

        [HttpGet("Tasks/{listId}")]
        [Authorize]
        public async Task<ActionResult<List<FullTaskDto>>> GetTaskAsync(Guid listId)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            var todoList = _currentUser.TaskManager.GetTodoList(listId);
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            return Ok(todoList.TaskList.Select(x => x.AsDto()));
        }

        [HttpGet("Task")]
        [Authorize]
        public async Task<ActionResult<List<FullTaskDto>>> GetTaskAsync(GetTaskDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            var todoList = _currentUser.TaskManager.GetTodoList(taskDto.ListId);
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }

            var task = todoList.TaskList.FirstOrDefault(x => x.Id == taskDto.TaskId);
            if (task is null)
            {
                return BadRequest("Task not found.");
            }
            return Ok(task.AsDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateTodoListAsync(CreateNewTodoListDto todoListDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            _currentUser.TaskManager.CreateNewTodoList(todoListDto.Name, todoListDto.Description);
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpPost("Task")]
        [Authorize]
        public async Task<ActionResult> CreateTaskAsync(CreateNewTaskDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryCreateNewTask(taskDto.ListId, taskDto.Name, taskDto.Description) is false)
            {
                return BadRequest("Todo list not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Start")]
        [Authorize]
        public async Task<ActionResult> StartTaskAsync(GetTaskDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryStartTask(taskDto.ListId, taskDto.TaskId) is false)
            {
                return BadRequest("Todo list or task not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Complete")]
        [Authorize]
        public async Task<ActionResult> CompleteTaskAsync(GetTaskDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryCompleteTask(taskDto.ListId, taskDto.TaskId) is false)
            {
                return BadRequest("Todo list or task not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Update/Status")]
        [Authorize]
        public async Task<ActionResult> UpdateTaskStatusAsync(UpdateTaskStatusDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryUpdateTaskStatus(taskDto.ListId, taskDto.TaskId, taskDto.Status) is false)
            {
                return BadRequest("Todo list or task not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Update/Name")]
        [Authorize]
        public async Task<ActionResult> UpdateTaskNameAsync(UpdateTaskNameDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryUpdateTaskName(taskDto.ListId, taskDto.TaskId, taskDto.Name) is false)
            {
                return BadRequest("Todo list or task not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Update/Description")]
        [Authorize]
        public async Task<ActionResult> UpdateTaskDescriptionAsync(UpdateTaskDescriptionDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryUpdateTaskDescription(taskDto.ListId, taskDto.TaskId, taskDto.Description) is false)
            {
                return BadRequest("Todo list or task not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpDelete("{listId}")]
        [Authorize]
        public async Task<ActionResult> DeleteTodoListAsync(Guid listId)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryDeleteList(listId) is false)
            {
                return BadRequest("Todo list not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpDelete("Task")]
        [Authorize]
        public async Task<ActionResult> DeleteTaskAsync(GetTaskDto taskDto)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryDeleteTask(taskDto.ListId, taskDto.TaskId) is false)
            {
                return BadRequest("Todo list or task not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }

        [HttpDelete("Tasks/All/{listId}")]
        [Authorize]
        public async Task<ActionResult> DeleteTasksAsync(Guid listId)
        {
            _currentUser = await _userRepository.GetUserAsync(User.FindFirstValue(ClaimTypes.Name));
            if (_currentUser.TaskManager.TryDeleteAllTasks(listId) is false)
            {
                return BadRequest("Todo list or task not found.");
            }
            await _userRepository.UpdateUserAsync(_currentUser);
            await _userRepository.SaveListAsync();
            return Ok();
        }
    }
}