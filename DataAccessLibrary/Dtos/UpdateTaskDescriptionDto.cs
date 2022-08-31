using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Dtos
{
    public record UpdateTaskDescriptionDto
    {
        public Guid Id { get; init; }
        public string Description { get; set; }
    }
}
