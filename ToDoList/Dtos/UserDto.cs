using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record UserDto
    {
#pragma warning disable CS8618
        [Required]
        [MinLength(4), MaxLength(20)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(6), MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}
