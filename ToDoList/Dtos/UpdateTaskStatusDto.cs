using DataAccessLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
#pragma warning disable CS8618
    public record UpdateTaskStatusDto
    {
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        public Guid? TaskId { get; set; }
        [Required]
        [Range(0,2)]
        public Status? Status { get; set; }
    }
}
