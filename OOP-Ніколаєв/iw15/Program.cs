using iw15;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Колекція 1: List<Product>
var products = new List<Product>
{
    new() { Name = "Laptop", Price = 25000m, Category = "Hardware" },
    new() { Name = "Mouse", Price = 500m, Category = "Peripherals" },
    new() { Name = "Mechanical Keyboard", Price = 2200m, Category = "Peripherals" },
    new() { Name = "USB Hub", Price = 850m, Category = "Accessories" }
};

Console.WriteLine("=== 1. Колекція List<Product> ===");
IEnumerable<string> productLabels = CollectionUtils.Map(products, p => $"{p.Name} ({p.Price} грн)");
Console.WriteLine("Map (назви з ціною): " + string.Join(", ", productLabels));

Product mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
Console.WriteLine($"MaxBy (найдорожчий товар): {mostExpensive.Name} — {mostExpensive.Price} грн");

IEnumerable<Product> expensiveProducts = CollectionUtils.Filter(products, p => p.Price >= 1000m);
Console.WriteLine("Filter (ціна >= 1000 грн) + ForEach:");
CollectionUtils.ForEach(expensiveProducts, p => Console.WriteLine($"  * {p.Name}: {p.Price} грн [{p.Category}]"));

// Колекція 2: List<int>
var numbers = new List<int> { 4, 19, 8, 42, 15, 3, 27 };

Console.WriteLine("\n=== 2. Колекція List<int> ===");
IEnumerable<int> squaredNumbers = CollectionUtils.Map(numbers, n => n * n);
Console.WriteLine("Map (квадрати чисел): " + string.Join(", ", squaredNumbers));

int maxNumber = CollectionUtils.MaxBy(numbers, n => n);
Console.WriteLine($"MaxBy (максимальне число): {maxNumber}");

IEnumerable<int> evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
Console.Write("Filter (парні числа): ");
CollectionUtils.ForEach(evenNumbers, n => Console.Write($"{n} "));
Console.WriteLine();

// Колекція 3: List<string>
var commands = new List<string> { "build", "test", "lint", "deploy-production", "clean" };

Console.WriteLine("\n=== 3. Колекція List<string> ===");
IEnumerable<string> upperCommands = CollectionUtils.Map(commands, cmd => cmd.ToUpperInvariant());
Console.WriteLine("Map (верхній регістр): " + string.Join(", ", upperCommands));

string longestCommand = CollectionUtils.MaxBy(commands, cmd => cmd.Length);
Console.WriteLine($"MaxBy (найдовший рядок): '{longestCommand}' (довжина {longestCommand.Length})");

IEnumerable<string> shortCommands = CollectionUtils.Filter(commands, cmd => cmd.Length <= 5);
Console.WriteLine("Filter (довжина <= 5 символів):");
CollectionUtils.ForEach(shortCommands, cmd => Console.WriteLine($"  - {cmd}"));
