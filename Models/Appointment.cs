using NanyPet.Api.Models.Common;

namespace NanyPet.Api.Models
{
    public partial class Appointment : AuditableBaseModel
    {
        public int? AnimalId { get; set; }
        public int? PetId { get; set; }
        public int? HerderId { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public string? Notes { get; set; }

        public virtual Herder? Herder { get; set; }
        public virtual Pet? Pet { get; set; }
    }
}
