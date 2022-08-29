using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class JsonTaskRepository : ITaskRepository
    {
        private List<TaskModel> _todoList { get; set; } = new();

        public JsonTaskRepository()
        {
            LoadList();
        }

        public async Task CreateNewTaskAsync(string name, string? description)
        {
            TaskModel task = new()
            {
                Name = name,
                Description = description,
                Status = Enums.Status.New
            };
            _todoList.Add(task);
            await SaveListAsync();
        }

        public async Task<bool> TryStartTaskAsync(Guid id)
        {
            var task = _todoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Status = Enums.Status.Current;
            task.StartedDate = DateTime.UtcNow;
            await SaveListAsync();
            return true;
        }

        public async Task<bool> TryCompleteTaskAsync(Guid id)
        {
            var task = _todoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Status = Enums.Status.Completed;
            task.CompletedDate = DateTime.UtcNow;
            await SaveListAsync();
            return true;
        }

        public async Task<bool> TryDeleteTaskAsync(Guid id)
        {
            var task = _todoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            _todoList.Remove(task);
            await SaveListAsync();
            return true;
        }

        public async Task<bool> TryUpdateTaskNameAsync(Guid id, string name)
        {
            var task = _todoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Name = name;
            task.UpdatedDate = DateTime.UtcNow;
            await SaveListAsync();
            return true;
        }

        public async Task<bool> TryUpdateTaskDescriptionAsync(Guid id, string descripiton)
        {
            var task = _todoList.Find(t => t.Id == id);
            if (task is null)
            {
                return false;
            }

            task.Description = descripiton;
            task.UpdatedDate = DateTime.UtcNow;
            await SaveListAsync();
            return true;
        }

        public List<TaskModel> GetTasks()
        {
            return _todoList;
        }

        public TaskModel GetTask(Guid id)
        {
            return _todoList.Find(t => t.Id == id);
        }

        private async Task LoadListAsync()
        {
            using FileStream fileStream = File.OpenRead("Tasks.json");
            _todoList = await JsonSerializer.DeserializeAsync<List<TaskModel>>(fileStream);
        }

        private void LoadList()
        {
            string jsonString = File.ReadAllText("Tasks.json");
            _todoList = JsonSerializer.Deserialize<List<TaskModel>>(jsonString);
        }

        private async Task SaveListAsync()
        {
            using FileStream fileStream = File.Create("Tasks.json");
            await JsonSerializer.SerializeAsync(fileStream, _todoList);
            await fileStream.DisposeAsync();
        }
    }
}
