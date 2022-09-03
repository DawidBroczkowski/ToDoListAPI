using DataAccessLibrary.Enums;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class TaskManager : ITaskManager
    {
        private Models.User _user;

        public TaskManager(Models.User user)
        {
            _user = user;
        }

        public void CreateNewTodoList(string name, string? description)
        {
            TodoList todoList = new()
            {
                Name = name,
                Description = description,
                TaskList = new()
            };
            _user.TodoLists.Add(todoList);
        }

        /// <summary>
        /// Creates a new task and adds it to the users Todo list.
        /// </summary>
        /// <param name="name">Name of the task.</param>
        /// <param name="description">Description of the task.</param>
        public bool TryCreateNewTask(Guid listId, string name, string? description)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return false;
            }

            Models.Task task = new()
            {
                Name = name,
                Description = description,
                Status = Status.New
            };

            todoList.TaskList.Add(task);
            return true;
        }

        /// <summary>
        /// Updates the task status to "Current".
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryStartTask(Guid listId, Guid id)
        {
            return TryUpdateTaskStatus(listId, id, Status.Current);
        }

        /// <summary>
        /// Updates the task status to "Completed".
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryCompleteTask(Guid listId, Guid id)
        {
            return TryUpdateTaskStatus(listId, id, Status.Completed);
        }

        public bool TryDeleteAllTasks(Guid listId)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return false;
            }

            todoList.TaskList.Clear();
            return true;
        }

        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryDeleteTask(Guid listId, Guid id)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return false;
            }

            var task = todoList.TaskList.FirstOrDefault(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            todoList.TaskList.Remove(task);
            return true;
        }

        public bool TryDeleteList(Guid listId)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return false;
            }

            return _user.TodoLists.Remove(todoList);
        }

        /// <summary>
        /// Updates the name of the task.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <param name="name">Updated task name.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryUpdateTaskName(Guid listId, Guid id, string name)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return false;
            }

            var task = todoList.TaskList.FirstOrDefault(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Name = name;
            task.UpdatedDate = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// Updates the name of the task.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <param name="descripiton">Updated task description.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryUpdateTaskDescription(Guid listId, Guid id, string descripiton)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return false;
            }

            var task = todoList.TaskList.FirstOrDefault(t => t.Id == id);
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
        /// <param name="id">Guid of the task.</param>
        /// <param name="status">Updated task status.</param>
        /// <returns>True if the task is found, false if not.</returns>
        public bool TryUpdateTaskStatus(Guid listId, Guid id, Status status)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return false;
            }

            var task = todoList.TaskList.FirstOrDefault(t => t.Id == id);
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

        /// <summary>
        /// Gets all tasks from the user.
        /// </summary>
        /// <returns>User's TodoList.</returns>
        public List<Models.Task>? GetTasks(Guid listId)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return null;
            }
            return todoList.TaskList;
        }

        /// <summary>
        /// Gets a task from the user.
        /// </summary>
        /// <param name="id">Guid of the task.</param>
        /// <returns></returns>
        public Models.Task? GetTask(Guid listId, Guid id)
        {
            var todoList = _user.TodoLists.FirstOrDefault(x => x.Id == listId);
            if (todoList is null)
            {
                return null;
            }
            return todoList.TaskList.FirstOrDefault(t => t.Id == id);
        }

        public List<TodoList>? GetTodoLists()
        {
            return _user.TodoLists;
        }

        public TodoList? GetTodoList(Guid listId)
        {
            return _user.TodoLists.FirstOrDefault(x => x.Id == listId);
        }
    }
}
