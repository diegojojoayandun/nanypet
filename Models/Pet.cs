using System;
using System.Collections.Generic;

namespace NanyPet.Models
{
    public partial class Pet
    {
        public Pet()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Species { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public int? OwnerId { get; set; }
        public string? Location { get; set; }

        public virtual Owner? Owner { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
