using DataAccessLibrary.Enums;

namespace DataAccessLibrary.Models
{
    public record TaskModel
    {
        public TaskModel()
        {
            CreatedDate = DateTime.UtcNow;
            Id = Guid.NewGuid();
        }

        public TaskModel(Guid id, string name, string? description, Status status, DateTime createdDate, DateTime? updatedDate, DateTime? startedDate, DateTime? completedDate)
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

        public Guid Id { get; init; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; init; }
        public DateTime? StartedDate { get; set; } = null;
        public DateTime? UpdatedDate { get; set; } = null;
        public DateTime? CompletedDate { get; set; } = null;
    }
}