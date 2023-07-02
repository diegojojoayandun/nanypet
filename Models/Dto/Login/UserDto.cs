namespace NanyPet.Api.Models.Dto.Login
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
