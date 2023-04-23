using DataAccessLibrary.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccessLibrary.Models
{
    public record Task
    {
#pragma warning disable CS8618
        public Task()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public Task(Guid id, string name, string? description, Status status, DateTime createdDate, DateTime? updatedDate, DateTime? startedDate, DateTime? completedDate)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            CreatedDate = createdDate;
            StartedDate = startedDate;
            UpdatedDate = updatedDate;
            CompletedDate = completedDate;
        }

        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public Status? Status { get; set; }
        [Required]
        public DateTime CreatedDate { get; init; }
        public DateTime? StartedDate { get; set; } = null;
        public DateTime? UpdatedDate { get; set; } = null;
        public DateTime? CompletedDate { get; set; } = null;
        [JsonIgnore]
        public virtual TodoList TodoList { get; set; }
    }
}