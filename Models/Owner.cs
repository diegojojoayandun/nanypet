using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanyPet.Api.Models
{
    public partial class Owner
    {
        public Owner()
        {
            Pets = new HashSet<Pet>();
        }
        [Key] // primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // autoincrement
        public int Id { get; set; }
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
