namespace SerilogLearningConsole;

public class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
}