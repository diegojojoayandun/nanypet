using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Location { get; set; } = null!;

        public virtual User EmailUserNavigation { get; set; } = null!;
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
