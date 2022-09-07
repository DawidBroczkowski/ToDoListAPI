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
        private Models.TodoList _todoList;

        public TaskManager(Models.TodoList todoList)
        {
            _todoList = todoList;
        }

        //public void CreateNewTodoList(string name, string? description)
        //{
        //    TodoList todoList = new()
        //    {
        //        Name = name,
        //        Description = description,
        //        TaskList = new()
        //    };
        //    _user.TodoLists.Add(todoList);
        //}

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

        public bool TryStartTask(Guid? id)
        {
            return TryUpdateTaskStatus(id, Status.Current);
        }

        public bool TryCompleteTask(Guid? id)
        {
            return TryUpdateTaskStatus(id, Status.Completed);
        }

        public bool TryDeleteAllTasks()
        {
            _todoList.TaskList.Clear();
            return true;
        }

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

        public bool TryUpdateTaskStatus(Guid? id, Status status)
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

        public List<Models.Task>? GetTasks()
        {
            return _todoList.TaskList;
        }

        public Models.Task? GetTask(Guid? id)
        {
            return _todoList.TaskList.FirstOrDefault(t => t.Id == id);
        }
    }
}
