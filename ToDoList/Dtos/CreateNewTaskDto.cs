using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
#pragma warning disable CS8618
    public record CreateNewTaskDto
    {
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
    }

}

