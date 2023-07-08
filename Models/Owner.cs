using NanyPet.Api.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanyPet.Api.Models
{
    public partial class Owner : AuditableBaseModel
    {
        public Owner()
        {
            Pets = new HashSet<Pet>();
        }
        [Required]
        [EmailAddress]
        [MaxLength(60)]
        public string EmailUser { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }

        public virtual User EmailUserNavigation { get; set; } = null!;
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
