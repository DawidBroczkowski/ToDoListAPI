using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record InviteDto
    {
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        public string ReceivingUsername { get; set; }
    }
}
