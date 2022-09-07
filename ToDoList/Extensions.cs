using DataAccessLibrary.Models;
using ToDoList.Dtos;

namespace ToDoList
{
    public static class Extensions
    {
        public static FullTaskDto AsDto(this DataAccessLibrary.Models.Task? task)
        {
             return new FullTaskDto()
             {
                 Id = task.Id,
                 Name = task.Name,
                 Description = task.Description,
                 Status = task.Status,
                 CreatedDate = task.CreatedDate,
                 StartedDate = task.StartedDate,
                 UpdatedDate = task.UpdatedDate,
                 CompletedDate = task.CompletedDate
             };
        }

        public static TodoListDto AsDto(this TodoList? todoList)
        {
            return new TodoListDto()
            {
                Id = todoList.Id,
                Name = todoList.Name,
                Description = todoList.Description,
                TaskList = todoList.TaskList
            };
        }

        public static GetInviteDto AsDto(this Invite? invite)
        {
            return new GetInviteDto()
            {
                ListId = invite.ListId,
                InviteId = invite.InviteId,
                InvitingUsername = invite.InvitingUsername,
                InviteDate = invite.InviteDate
            };
        }
    }
}
