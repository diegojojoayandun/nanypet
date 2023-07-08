using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NanyPet.Api.Models.Common
{
    public abstract class AuditableBaseModel
    {
        [Key] // primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // autoincrement
        public virtual int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
