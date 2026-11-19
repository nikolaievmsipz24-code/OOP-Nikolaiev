using IndependentWork7;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var cache = new Cache<string, string>(capacity: 3);

Console.WriteLine("=== Додавання елементів у кеш (capacity = 3) ===");
cache.Add("user:1", "Alice");
cache.Add("user:2", "Bob");
cache.Add("user:3", "Charlie");
Console.WriteLine($"Поточна кількість елементів (Count): {cache.Count}");
Console.WriteLine($"Отримано user:1 -> {cache.Get("user:1")}");

Console.WriteLine("\n=== Перевищення ємності (додаємо user:4, має витіснитися user:1) ===");
cache.Add("user:4", "Diana");
Console.WriteLine($"Count після додавання 4-го елемента: {cache.Count}");
Console.WriteLine($"ContainsKey('user:1'): {cache.ContainsKey("user:1")}");
Console.WriteLine($"ContainsKey('user:4'): {cache.ContainsKey("user:4")}");

try
{
    _ = cache.Get("user:1");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Очікуваний виняток при зверненні до видаленого ключа: {ex.Message}");
}