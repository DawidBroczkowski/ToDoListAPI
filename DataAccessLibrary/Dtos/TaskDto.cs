using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Dtos
{
    public record TaskDto
    {
        [Required]
        public Guid Id { get; init; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
