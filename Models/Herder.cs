using NanyPet.Api.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanyPet.Api.Models
{
    public partial class Herder : AuditableBaseModel
    {
        public Herder()
        {
            Appointments = new HashSet<Appointment>();
        }
        [Required]
        [EmailAddress]
        [StringLength(60)]
        public string EmailUser { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }

        public virtual User EmailUserNavigation { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
