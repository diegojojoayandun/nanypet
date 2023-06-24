using System;
using System.Collections.Generic;

namespace NanyPet.Api.Models
{
    public partial class Pet
    {
        public Pet()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public int? OwnerId { get; set; }

        public virtual Owner Owner { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
