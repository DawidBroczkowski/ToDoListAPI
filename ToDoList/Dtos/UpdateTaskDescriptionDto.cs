using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record UpdateTaskDescriptionDto
    {
#pragma warning disable CS8618
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        public Guid? TaskId { get; init; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
