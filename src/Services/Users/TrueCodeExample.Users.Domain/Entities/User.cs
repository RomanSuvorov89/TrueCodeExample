namespace TrueCodeExample.Users.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public User(string name, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        PasswordHash = passwordHash;
    }

    private User(Guid id, string name, string passwordHash)
    {
        Id = id;
        Name = name;
        PasswordHash = passwordHash;
    }

    public static User Restore(Guid id, string name, string passwordHash) => new(id, name, passwordHash);
}
