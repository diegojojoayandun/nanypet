using System.ComponentModel.DataAnnotations;

namespace NanyPet.Models.Dto.Owner
{
    public class OwnerUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? EmailUser { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
