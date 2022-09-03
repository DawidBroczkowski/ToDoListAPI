using DataAccessLibrary.Enums;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public interface ITaskManager
    {
        void CreateNewTodoList(string name, string? description);
        Models.Task? GetTask(Guid listId, Guid id);
        List<Models.Task>? GetTasks(Guid listId);
        TodoList? GetTodoList(Guid listId);
        List<TodoList>? GetTodoLists();
        bool TryCompleteTask(Guid listId, Guid id);
        bool TryCreateNewTask(Guid listId, string name, string? description);
        bool TryDeleteAllTasks(Guid listId);
        bool TryDeleteTask(Guid listId, Guid id);
        bool TryDeleteList(Guid listId);
        bool TryStartTask(Guid listId, Guid id);
        bool TryUpdateTaskDescription(Guid listId, Guid id, string descripiton);
        bool TryUpdateTaskName(Guid listId, Guid id, string name);
        bool TryUpdateTaskStatus(Guid listId, Guid id, Status status);
    }
}