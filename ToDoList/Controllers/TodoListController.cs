using DataAccessLibrary.DataAccess;
using ToDoList.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DataAccessLibrary.Models;
using DataAccessLibrary.DataAccess.DbData;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoListController : Controller
    {
        private User? _currentUser;
        private IRepository? _repository;

        public TodoListController(ListContext db, IRepository Repository)
        {
            _repository = Repository;
            _repository.SetContext(db);
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<TodoListDto>> GetAllTodoListsAsync()
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoLists = await _repository!.GetUserOwnTodoListsAsync(_currentUser!.Username)!;
            todoLists!.AddRange((await _repository!.GetUserCollabTodoListsAsync(_currentUser.Username)!)!);
            return todoLists.Select(t => t.AsDto());
        }

        [HttpGet("Own")]
        [Authorize]
        public async Task<IEnumerable<TodoListDto>> GetOwnTodoListsAsync()
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoLists = await _repository!.GetUserOwnTodoListsAsync(_currentUser!.Username)!;
            return todoLists!.Select(t => t.AsDto());
        }

        [HttpGet("Collabs")]
        [Authorize]
        public async Task<IEnumerable<TodoListDto>> GetCollabTodoListsAsync()
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoLists = await _repository.GetUserCollabTodoListsAsync(_currentUser!.Username)!;
            return todoLists!.Select(t => t.AsDto());
        }

        [HttpGet("{listId}")]
        [Authorize]
        public async Task<ActionResult<TodoListDto>> GetTodoListAsync(Guid listId)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndTaskListAsync(_currentUser!.Username, listId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            return Ok(todoList.AsDto());
        }

        [HttpGet("Tasks/{listId}")]
        [Authorize]
        public async Task<ActionResult<List<FullTaskDto>>> GetTaskAsync(Guid listId)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndTaskListAsync(_currentUser!.Username, listId)!;
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
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndSingleTaskAsync(_currentUser!.Username, taskDto.ListId, taskDto.TaskId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }

            var task = todoList.TaskList.FirstOrDefault();
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
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            await _repository.CreateNewTodoListAsync(_currentUser!.Username, todoListDto.Name, todoListDto.Description);
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPost("Task")]
        [Authorize]
        public async Task<ActionResult> CreateTaskAsync(CreateNewTaskDto taskDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAsync(_currentUser!.Username, taskDto.ListId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }

            todoList.TaskList = new();
            todoList.TaskManager.CreateNewTask(taskDto.Name, taskDto.Description);
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Start")]
        [Authorize]
        public async Task<ActionResult> StartTaskAsync(GetTaskDto taskDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndSingleTaskAsync(_currentUser!.Username, taskDto.ListId, taskDto.TaskId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            if(todoList.TaskManager.TryStartTask(taskDto.TaskId) is false)
            {
                return BadRequest("Task not found.");
            }
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Complete")]
        [Authorize]
        public async Task<ActionResult> CompleteTaskAsync(GetTaskDto taskDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndSingleTaskAsync(_currentUser!.Username, taskDto.ListId, taskDto.TaskId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            if (todoList.TaskManager.TryCompleteTask(taskDto.TaskId) is false)
            {
                return BadRequest("Task not found.");
            }
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Update/Status")]
        [Authorize]
        public async Task<ActionResult> UpdateTaskStatusAsync(UpdateTaskStatusDto taskDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndSingleTaskAsync(_currentUser!.Username, taskDto.ListId, taskDto.TaskId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            if (todoList.TaskManager.TryUpdateTaskStatus(taskDto.TaskId, taskDto.Status) is false)
            {
                return BadRequest("Task not found.");
            }
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Update/Name")]
        [Authorize]
        public async Task<ActionResult> UpdateTaskNameAsync(UpdateTaskNameDto taskDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndSingleTaskAsync(_currentUser!.Username, taskDto.ListId, taskDto.TaskId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            if (todoList.TaskManager.TryUpdateTaskName(taskDto.TaskId, taskDto.Name) is false)
            {
                return BadRequest("Task not found.");
            }
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Task/Update/Description")]
        [Authorize]
        public async Task<ActionResult> UpdateTaskDescriptionAsync(UpdateTaskDescriptionDto taskDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndSingleTaskAsync(_currentUser!.Username, taskDto.ListId, taskDto.TaskId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            if (todoList.TaskManager.TryUpdateTaskDescription(taskDto.TaskId, taskDto.Description) is false)
            {
                return BadRequest("Task not found.");
            }
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpDelete("{listId}")]
        [Authorize]
        public async Task<ActionResult> DeleteTodoListAsync(Guid listId)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAsync(_currentUser!.Username, listId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }

            var owner = await _repository.GetTodoListOwnerAsync(todoList.Id)!;
            if (_currentUser.Username != owner!.Username)
            {
                return BadRequest("You are not the owner.");
            }

            _repository.RemoveTodoList(todoList);
            await _repository.SaveListAsync();   
            return Ok();
        }

        [HttpDelete("Task")]
        [Authorize]
        public async Task<ActionResult> DeleteTaskAsync(GetTaskDto taskDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndSingleTaskAsync(_currentUser!.Username, taskDto.ListId, taskDto.TaskId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            var task = todoList.TaskList.FirstOrDefault();
            if (task is null)
            {
                return BadRequest("Task not found.");
            }
            todoList.TaskManager.TryDeleteTask(taskDto.TaskId);
            //_userRepository.DeleteObject(task);
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpDelete("Tasks/All/{listId}")]
        [Authorize]
        public async Task<ActionResult> DeleteAllTasksAsync(Guid listId)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var todoList = await _repository.GetTodoListAndTaskListAsync(_currentUser!.Username, listId)!;
            if (todoList is null)
            {
                return BadRequest("List not found.");
            }
            todoList.TaskManager.DeleteAllTasks();
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPost("Invite")]
        [Authorize]
        public async Task<ActionResult> InviteUserAsync(InviteDto inviteDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var owner = await _repository.GetTodoListOwnerAsync(inviteDto.ListId)!;
            if (owner is null)
            {
                return BadRequest("List not found.");
            }
            if (owner.Username != _currentUser!.Username)
            {
                return BadRequest("Acces denied. You are not the list's owner.");
            }

            var collaborators = await _repository.GetTodoListCollaboratorsAsync(inviteDto.ListId)!;
            if (collaborators?.FirstOrDefault(x => x!.Username == inviteDto.ReceivingUsername) is not null)
            {
                return BadRequest("User is already a collaborator.");
            }

            if (await _repository.TryInviteToCollabAsync(_currentUser.Username, inviteDto.ReceivingUsername, inviteDto.ListId) is false)
            {
                return BadRequest("User not found.");
            }

            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpGet("Invite")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetInviteDto>>> GetInvitesAsync()
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var invites = await _repository!.GetUserInvitesAsync(_currentUser!.Username)!;
            return Ok(invites!.Select(x => x.AsDto()));
        }

        [HttpPut("Invite/Accept/{inviteId}")]
        [Authorize]
        public async Task<ActionResult> AcceptInviteAsync(Guid? inviteId)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            if (await _repository.TryAcceptInvite(_currentUser!.Username, inviteId) is false)
            {
                return BadRequest("Invite not found.");
            }
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpPut("Invite/Decline/{inviteId}")]
        [Authorize]
        public async Task<ActionResult> DeclineInviteAsync(Guid? inviteId)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            if (await _repository.TryDeclineInvite(_currentUser!.Username, inviteId) is false)
            {
                return BadRequest("Invite not found.");
            }
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpDelete("Collab")]
        [Authorize]
        public async Task<ActionResult> RemoveFromCollabAsync(RemoveFromCollabDto collabDto)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;
            var owner = await _repository.GetTodoListOwnerAsync(collabDto.ListId)!;
            if (owner is null)
            {
                return BadRequest("List not found.");
            }
            if (owner.Username != _currentUser!.Username)
            {
                return BadRequest("Acces denied. You are not the list's owner.");
            }

            var collaborators = await _repository.GetTodoListCollaboratorsAsync(collabDto.ListId)!;
            if (collaborators?.FirstOrDefault(x => x!.Username == collabDto.Username) is null)
            {
                return BadRequest("User is not a collaborator.");
            }

            await _repository.TryRemoveFromCollabAsync(collabDto.Username, collabDto.ListId);
            await _repository.SaveListAsync();
            return Ok();
        }

        [HttpDelete("Collab/Self/{listId}")]
        [Authorize]
        public async Task<ActionResult> RemoveSelfFromCollabAsync(Guid listId)
        {
            _currentUser = await _repository!.GetUserAsync(User.FindFirstValue(ClaimTypes.Name))!;

            var collaborators = await _repository.GetTodoListCollaboratorsAsync(listId)!;
            if (collaborators!.FirstOrDefault(x => x!.Username == _currentUser!.Username) is null)
            {
                return BadRequest("Collab not found.");
            }

            await _repository.TryRemoveFromCollabAsync(_currentUser!.Username, listId);
            await _repository.SaveListAsync();
            return Ok();
        }
    }
}