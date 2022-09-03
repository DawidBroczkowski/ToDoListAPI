using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Dtos
{
    public record UpdateTaskDescriptionDto
    {
        [Required]
        public Guid ListId { get; set; }
        [Required]
        public Guid TaskId { get; init; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
