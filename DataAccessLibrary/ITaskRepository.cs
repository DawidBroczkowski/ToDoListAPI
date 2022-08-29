using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ITaskRepository
    {
        Task<bool> TryCompleteTaskAsync(Guid id);
        Task CreateNewTaskAsync(string name, string? description);
        Task<bool> TryDeleteTaskAsync(Guid id);
        TaskModel GetTask(Guid id);
        List<TaskModel> GetTasks();
        Task<bool> TryStartTaskAsync(Guid id);
        Task<bool> TryUpdateTaskDescriptionAsync(Guid id, string descripiton);
        Task<bool> TryUpdateTaskNameAsync(Guid id, string name);
    }
}