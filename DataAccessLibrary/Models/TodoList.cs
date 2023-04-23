using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public record TodoList
    {
#pragma warning disable CS8618
        public TodoList()
        {
            TaskManager = new TaskManager(this);
        }

        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string? Description { get; set; }
        public virtual List<Models.Task> TaskList { get; set; }
        public virtual Models.User Owner { get; set; }
        public virtual List<Models.Collab> Collaborations { get; set; }
        [JsonIgnore]
        [NotMapped]
        public ITaskManager TaskManager { get; set; }
    }
}
