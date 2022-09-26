namespace eRS.Models.Models.Users;

public class AuthenticatedResponse
{
    public string Token { get; set; }
    public DateTime ValidTo { get; set; }
}