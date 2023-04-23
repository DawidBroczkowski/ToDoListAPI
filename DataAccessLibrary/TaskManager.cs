using DataAccessLibrary.Enums;

namespace DataAccessLibrary
{
    public class TaskManager : ITaskManager
    {
        private Models.TodoList _todoList;

        public TaskManager(Models.TodoList todoList)
        {
            _todoList = todoList;
        }

        /// <summary>
        /// Adds a task to the task list.
        /// </summary>
        /// <param name="name">Name of the task.</param>
        /// <param name="description">Description of the task</param>
        public void CreateNewTask(string name, string? description)
        {
            Models.Task task = new()
            {
                Name = name,
                Description = description,
                Status = Status.New
            };

            _todoList.TaskList.Add(task);
        }

        /// <summary>
        /// Updates the task's status to "started".
        /// </summary>
        /// <param name="id">Id of the task.</param>
        /// <returns><see langword="true"/> if the update succeeded, <see langword="false"/> if a task was not found.</returns>
        public bool TryStartTask(Guid? id)
        {
            return TryUpdateTaskStatus(id, Status.Current);
        }

        /// <summary>
        /// Updates the task's status to "completed".
        /// </summary>
        /// <param name="id">Id of the task.</param>
        /// <returns><see langword="true"/> if the update succeeded, <see langword="false"/> if task was not found.</returns>
        public bool TryCompleteTask(Guid? id)
        {
            return TryUpdateTaskStatus(id, Status.Completed);
        }

        /// <summary>
        /// Clears the task list.
        /// </summary>
        public void DeleteAllTasks()
        {
            _todoList.TaskList.Clear();
        }

        /// <summary>
        /// Deletes a task from the list.
        /// </summary>
        /// <param name="id">Id of the task.</param>
        /// <returns><see langword="true"/> if the update succeeded, <see langword="false"/> if task was not found.</returns>
        public bool TryDeleteTask(Guid? id)
        {
            var task = _todoList.TaskList.FirstOrDefault(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            _todoList.TaskList.Remove(task);
            return true;
        }

        /// <summary>
        /// Updates the task's name.
        /// </summary>
        /// <param name="id">Id of the task.</param>
        /// <param name="name">New name.</param>
        /// <returns><see langword="true"/> if the update succeeded, <see langword="false"/> if task was not found.</returns>
        public bool TryUpdateTaskName(Guid? id, string name)
        {
            var task = _todoList.TaskList.FirstOrDefault(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Name = name;
            task.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// Updates the task's description.
        /// </summary>
        /// <param name="id">Id of the task.</param>
        /// <param name="name">New description.</param>
        /// <returns><see langword="true"/> if the update succeeded, <see langword="false"/> if task was not found.</returns>
        public bool TryUpdateTaskDescription(Guid? id, string descripiton)
        {
            var task = _todoList.TaskList.FirstOrDefault(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Description = descripiton;
            task.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// Updates the task status.
        /// </summary>
        /// <param name="id">Id of the task.</param>
        /// <param name="status">New status.</param>
        /// <returns><see langword="true"/> if the update succeeded, <see langword="false"/> if a task was not found.</returns>
        public bool TryUpdateTaskStatus(Guid? id, Status? status)
        {
            var task = _todoList.TaskList.FirstOrDefault(t => t.Id == id);
            if (task is null)
            {
                return false;
            }
            if (task.Status == status)
            {
                return true;
            }
            else if (task.Status is Status.Completed && status is Status.Current)
            {
                task.CompletedDate = null;
            }
            else if (task.Status is Status.Completed && status is Status.New)
            {
                task.CompletedDate = null;
                task.StartedDate = null;
            }
            else if (task.Status is Status.Current && status is Status.New)
            {
                task.StartedDate = null;
            }
            else if (task.Status is Status.Current && status is Status.Completed)
            {
                task.CompletedDate = DateTime.UtcNow;
            }
            else if (task.Status is Status.New && status is Status.Current)
            {
                task.StartedDate = DateTime.UtcNow;
            }
            else if (task.Status is Status.New && status is Status.Completed)
            {
                task.StartedDate = DateTime.UtcNow;
                task.CompletedDate = DateTime.UtcNow;
            }

            task.UpdatedDate = DateTime.UtcNow;
            task.Status = status;
            return true;
        }
    }
}
