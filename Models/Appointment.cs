using System;
using System.Collections.Generic;

namespace NanyPet.Api.Models
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public int? AnimalId { get; set; }
        public int? PetId { get; set; }
        public int? HerderId { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public string? Notes { get; set; }

        public virtual Herder? Herder { get; set; }
        public virtual Pet? Pet { get; set; }
    }
}
