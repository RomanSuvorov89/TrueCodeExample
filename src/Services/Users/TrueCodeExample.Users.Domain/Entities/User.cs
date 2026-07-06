namespace TrueCodeExample.Users.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    private User()
    {
    }

    public User(string name, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        PasswordHash = passwordHash;
    }
}
