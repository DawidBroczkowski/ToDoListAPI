using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Models
{
    public record Collab
    {
#pragma warning disable CS8618
        [Key]
        [Required]
        public Guid CollabId { get; set; }
        [Required]
        public virtual Models.User User { get; set; }
        [Required]
        public virtual Models.TodoList TodoList { get; set; }
    }
}
