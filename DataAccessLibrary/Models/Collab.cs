using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public record Collab
    {
        [Key]
        [Required]
        public Guid CollabId { get; set; }
        [Required]
        public virtual Models.User User { get; set; }
        [Required]
        public virtual Models.TodoList TodoList { get; set; }
    }
}
