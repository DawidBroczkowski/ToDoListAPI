using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Dtos
{
    public record UpdateTaskNameDto
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
    }
}
