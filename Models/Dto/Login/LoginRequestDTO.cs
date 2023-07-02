namespace NanyPet.Api.Models.Dto.Login
{
    public class LoginRequestDTO
    {
        public string EmailUser { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
