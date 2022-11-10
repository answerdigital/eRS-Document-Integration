namespace eRS.Data.Entities;

public sealed class User
{
    public int UserRowID { get; set; }
    public Guid UserReference { get; set; } = Guid.NewGuid();
    public string UserEmail { get; set; }
    public string UserPassword { get; set; }
    public string? UserForename { get; set; }
    public string? UserSurname { get; set; }
}
