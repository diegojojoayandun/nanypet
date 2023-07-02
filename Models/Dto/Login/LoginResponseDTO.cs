namespace NanyPet.Api.Models.Dto.Login
{
    public class LoginResponseDTO
    {
        public User? User { get; set; }
        public string? Token { get; set; } = null!;
    }
}
