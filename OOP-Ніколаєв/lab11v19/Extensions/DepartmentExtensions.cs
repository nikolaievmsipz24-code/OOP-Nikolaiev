using lab11v19.Models;

namespace lab11v19.Extensions;

public static class DepartmentExtensions
{
    public static decimal AverageDepartmentSalary(this Department department)
    {
        ArgumentNullException.ThrowIfNull(department);

        if (department.Employees is null || department.Employees.Count == 0)
            return 0m;

        return department.Employees.Average(e => e.Salary);
    }
}