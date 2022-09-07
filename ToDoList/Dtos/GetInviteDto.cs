using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{
    public record GetInviteDto
    {
        [Required]
        public Guid? InviteId { get; set; }
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        public string InvitingUsername { get; set; }
        [Required]
        public DateTime InviteDate { get; set; }
    }
}
