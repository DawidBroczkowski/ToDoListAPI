using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Dtos
{
    public record UpdateTaskStatusDto
    {
        [Required]
        public Guid ListId { get; set; }
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        [Range(0,2)]
        public Status Status { get; set; }
    }
}
