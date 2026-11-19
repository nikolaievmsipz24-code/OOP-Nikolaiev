namespace lab11v19.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }

    public override string ToString() => $"[{Id}] {Name} — {Salary:N0} грн";
}