using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public record Invite
    {
#pragma warning disable CS8618
        [Key]
        [Required]
        public Guid? InviteId { get; set; }
        [Required]
        public Guid? ListId { get; set; }
        [Required]
        [MaxLength(30)]
        public string InvitingUsername { get; set; }
        [Required]
        public DateTime InviteDate { get; set; }
        [Required]
        [JsonIgnore]
        public virtual User TargetUser { get; set; }
    }
}
