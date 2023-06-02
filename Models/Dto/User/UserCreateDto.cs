namespace NanyPet.Api.Models.Dto.User
{
    public class UserCreateDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
    }
}
