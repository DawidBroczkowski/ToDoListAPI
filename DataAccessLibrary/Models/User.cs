using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary;

namespace DataAccessLibrary.Models
{
    public record User
    {
        public User()
        {
            TaskManager = new TaskManager(this);
        }

        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        [MaxLength(128)]
        public byte[] PasswordHash { get; set; }
        [Required]
        [MaxLength(256)]
        public byte[] PasswordSalt { get; set; }
        public List<TodoList>? TodoLists { get; set; }
        [JsonIgnore]
        [NotMapped]
        public ITaskManager TaskManager { get; set; }
    }
}
