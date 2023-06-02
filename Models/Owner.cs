using System;
using System.Collections.Generic;

namespace NanyPet.Api.Models
{
    public partial class Owner
    {
        public Owner()
        {
            Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }
        public string EmailUser { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Location { get; set; }

        public virtual User EmailUserNavigation { get; set; } = null!;
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
