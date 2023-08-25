namespace eRS.Models.Models.Users;

public class UserCreate
{
    public string UserEmail { get; set; }
    public string UserPassword { get; set; }
    public string? UserForename { get; set; }
    public string? UserSurname { get; set; }
}