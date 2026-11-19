using System.Text;
using lab11v19.Extensions;
using lab11v19.Models;

Console.OutputEncoding = Encoding.UTF8;

List<Company> companies = SeedData();


Console.WriteLine("=== 1. Усі працівники всіх компаній (SelectMany) ===");
var allEmployees = companies
    .SelectMany(c => c.Departments)
    .SelectMany(d => d.Employees)
    .ToList();

Console.WriteLine($"Загальна кількість працівників: {allEmployees.Count}");
foreach (var emp in allEmployees)
{
    Console.WriteLine($"  - {emp}");
}


var allDepartments = companies
    .SelectMany(c => c.Departments, (company, dept) => new { CompanyName = company.Name, Department = dept })
    .ToList();


Console.WriteLine("\n=== 2. Пагінація відділів (по 3 на сторінку) ===");
const int pageSize = 3;
int totalDepartments = allDepartments.Count;
int totalPages = (int)Math.Ceiling(totalDepartments / (double)pageSize);

for (int page = 1; page <= Math.Min(3, totalPages); page++)
{
    var pageItems = allDepartments
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    Console.WriteLine($"\n--- Сторінка {page} з {totalPages} ---");
    foreach (var item in pageItems)
    {
        Console.WriteLine($"  {item.Department} | Компанія: {item.CompanyName}");
    }
}

Console.WriteLine("\n=== 3. Середня зарплата у відділах (AverageDepartmentSalary) ===");
foreach (var item in allDepartments)
{
    decimal avgSalary = item.Department.AverageDepartmentSalary();
    Console.WriteLine($"  {item.Department.Name} ({item.CompanyName}): {avgSalary:N2} грн");
}

static List<Company> SeedData() =>
[
    new Company
    {
        Id = 1,
        Name = "TechNova",
        Departments =
        [
            new Department
            {
                Id = 101,
                Name = "Backend Development",
                Employees =
                [
                    new Employee { Id = 1, Name = "Олена Коваль", Salary = 65000m },
                    new Employee { Id = 2, Name = "Максим Ткаченко", Salary = 72000m },
                    new Employee { Id = 3, Name = "Ірина Шевчук", Salary = 58000m }
                ]
            },
            new Department
            {
                Id = 102,
                Name = "QA Automation",
                Employees =
                [
                    new Employee { Id = 4, Name = "Денис Мельник", Salary = 45000m },
                    new Employee { Id = 5, Name = "Софія Бондар", Salary = 49000m }
                ]
            },
            new Department
            {
                Id = 103,
                Name = "DevOps",
                Employees =
                [
                    new Employee { Id = 6, Name = "Артем Кравченко", Salary = 78000m },
                    new Employee { Id = 7, Name = "Вікторія Савчук", Salary = 74000m }
                ]
            }
        ]
    },
    new Company
    {
        Id = 2,
        Name = "FinCore UA",
        Departments =
        [
            new Department
            {
                Id = 201,
                Name = "Risk Analytics",
                Employees =
                [
                    new Employee { Id = 8, Name = "Андрій Поліщук", Salary = 62000m },
                    new Employee { Id = 9, Name = "Юлія Марченко", Salary = 60000m },
                    new Employee { Id = 10, Name = "Назар Лисенко", Salary = 55000m }
                ]
            },
            new Department
            {
                Id = 202,
                Name = "Customer Support",
                Employees =
                [
                    new Employee { Id = 11, Name = "Катерина Гнатюк", Salary = 32000m },
                    new Employee { Id = 12, Name = "Богдан Олійник", Salary = 34000m }
                ]
            },
            new Department
            {
                Id = 203,
                Name = "Legal & Compliance",
                Employees =
                [
                    new Employee { Id = 13, Name = "Марія Павленко", Salary = 51000m },
                    new Employee { Id = 14, Name = "Тарас Руденко", Salary = 53000m }
                ]
            }
        ]
    },
    new Company
    {
        Id = 3,
        Name = "GreenLogistics",
        Departments =
        [
            new Department
            {
                Id = 301,
                Name = "Supply Chain",
                Employees =
                [
                    new Employee { Id = 15, Name = "Олег Захарчук", Salary = 44000m },
                    new Employee { Id = 16, Name = "Дарина Мороз", Salary = 46000m },
                    new Employee { Id = 17, Name = "Роман Кузьменко", Salary = 42000m }
                ]
            },
            new Department
            {
                Id = 302,
                Name = "Fleet Management",
                Employees =
                [
                    new Employee { Id = 18, Name = "Василь Хоменко", Salary = 39000m },
                    new Employee { Id = 19, Name = "Анастасія Демчук", Salary = 41000m }
                ]
            },
            new Department
            {
                Id = 303,
                Name = "HR & Talent",
                Employees =
                [
                    new Employee { Id = 20, Name = "Інна Гаврилюк", Salary = 38000m },
                    new Employee { Id = 21, Name = "Павло Клименко", Salary = 40000m }
                ]
            }
        ]
    }
];
