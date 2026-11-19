namespace lab11v19.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Department> Departments { get; set; } = new();

    public override string ToString() => $"[{Id}] {Name} (відділів: {Departments.Count})";
}