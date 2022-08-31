using DataAccessLibrary.Enums;

namespace DataAccessLibrary
{
    public interface ITaskManager
    {
        void CreateNewTask(string name, string? description);
        Models.Task GetTask(Guid id);
        List<Models.Task> GetTasks();
        bool TryCompleteTask(Guid id);
        bool TryDeleteTask(Guid id);
        bool TryStartTask(Guid id);
        bool TryUpdateTaskDescription(Guid id, string descripiton);
        bool TryUpdateTaskName(Guid id, string name);
        bool TryUpdateTaskStatus(Guid id, Status status);
    }
}