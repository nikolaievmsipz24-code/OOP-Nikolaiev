using iw20;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var retryPolicy = new RetryPolicy(
    maxRetries: 3,
    initialDelay: TimeSpan.FromMilliseconds(200),
    jitterFactor: 0.25);

Func<Exception, bool> isTransientError = ex => ex is HttpRequestException or TimeoutException;

Console.WriteLine("=== Сценарій 1: Операція з першими невдачами, яка зрештою завершується успішно ===");
int flakyAttempts = 0;

string payload = retryPolicy.Execute(() =>
{
    flakyAttempts++;
    if (flakyAttempts < 3)
    {
        throw new HttpRequestException($"Тимчасова помилка мережі #{flakyAttempts}");
    }

    return $"DATA-OK (успіх на спробі #{flakyAttempts})";
}, isTransientError);

Console.WriteLine($"Результат сценарію 1: {payload}\n");

Console.WriteLine("=== Сценарій 2: Операція, яка завжди завершується невдачею (остаточний виняток) ===");
int alwaysFailingAttempts = 0;

try
{
    retryPolicy.Execute<int>(() =>
    {
        alwaysFailingAttempts++;
        throw new HttpRequestException($"Сервіс недоступний на спробі #{alwaysFailingAttempts}");
    }, isTransientError);
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Результат сценарію 2: перехоплено фінальний виняток після {alwaysFailingAttempts} запусків -> {ex.Message}\n");
}

Console.WriteLine("=== Сценарій 3: Нетимчасова помилка (shouldRetry повертає false — без повторів) ===");
try
{
    retryPolicy.Execute<string>(() => throw new ArgumentException("Некоректні вхідні параметри запиту."), isTransientError);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Результат сценарію 3: перехоплено без повторів -> {ex.Message}");
}
