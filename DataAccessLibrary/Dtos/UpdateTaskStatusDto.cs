using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Dtos
{
    public record UpdateTaskStatusDto
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
    }
}
