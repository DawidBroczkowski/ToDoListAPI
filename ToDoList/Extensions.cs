using DataAccessLibrary.Dtos;
using DataAccessLibrary.Models;

namespace ToDoList
{
    public static class Extensions
    {
       public static FullTaskDto AsDto(this TaskModel task)
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
    }
}
