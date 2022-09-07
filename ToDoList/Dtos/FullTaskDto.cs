using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Dtos
{
    public record FullTaskDto
    {
        public Guid? Id { get; init; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; init; }
        public DateTime? StartedDate { get; set; } = null;
        public DateTime? UpdatedDate { get; set; } = null;
        public DateTime? CompletedDate { get; set; } = null;
    }
}
