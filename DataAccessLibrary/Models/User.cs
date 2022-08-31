using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataAccessLibrary;

namespace DataAccessLibrary.Models
{
    public record User
    {
        public User()
        {
            TaskManager = new JsonTaskManager(this);
        }

        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<Task> TodoList { get; set; } = new();
        [JsonIgnore]
        public ITaskManager TaskManager { get; set; }
    }
}
