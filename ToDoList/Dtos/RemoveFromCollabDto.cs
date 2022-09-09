using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record RemoveFromCollabDto
    {
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        [MaxLength(30)]
        public string Username { get; set; } 
    }
}
