namespace NanyPet.Api.Models.Dto.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
