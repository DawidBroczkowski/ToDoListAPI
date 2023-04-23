using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record GetTaskDto
    {
#pragma warning disable CS8618
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        public Guid? TaskId { get; set; }
    }
}
