using DataAccessLibrary.Enums;

namespace ToDoList.Dtos
{
    public record FullTaskDto
    {
#pragma warning disable CS8618
        public Guid? Id { get; init; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Status? Status { get; set; }
        public DateTime CreatedDate { get; init; }
        public DateTime? StartedDate { get; set; } = null;
        public DateTime? UpdatedDate { get; set; } = null;
        public DateTime? CompletedDate { get; set; } = null;
    }
}
