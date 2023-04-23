using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record InviteDto
    {
#pragma warning disable CS8618
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        public string ReceivingUsername { get; set; }
    }
}
