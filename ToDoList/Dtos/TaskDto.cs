using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record TaskDto
    {
#pragma warning disable CS8618
        [Required]
        public Guid? Id { get; init; }
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
