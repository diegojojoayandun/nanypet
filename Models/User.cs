using NanyPet.Api.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanyPet.Api.Models
{
    public partial class User : AuditableBaseModel
    {
        public User()
        {
            Herders = new HashSet<Herder>();
        }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email Address is requiered")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is requiered")]
        public string Password { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public virtual Owner? Owner { get; set; }
        public virtual ICollection<Herder> Herders { get; set; }
    }
}
