namespace Domain;

public class BaseEntity
{
    public int Id { get; set; }
}

public class Person : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
}