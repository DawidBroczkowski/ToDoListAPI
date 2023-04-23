using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record UpdateTaskNameDto
    {
#pragma warning disable CS8618
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        public Guid? TaskId { get; init; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
