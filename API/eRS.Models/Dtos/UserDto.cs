namespace eRS.Models.Dtos;

public sealed class UserDto
{
    public Guid? UserReference { get; set; }
    public string UserEmail { get; set; }
    public string? UserForename { get; set; }
    public string? UserSurname { get; set; }
    public string? UserFullName { get; set; }
}
