using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Dtos
{
    public record UserDto
    {
        [Required]
        [MinLength(4), MaxLength(20)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(6), MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}
