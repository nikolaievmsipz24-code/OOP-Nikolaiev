using lab18v19;

Console.OutputEncoding = System.Text.Encoding.UTF8;

static bool ShouldRetryDatabaseException(Exception ex) =>
    ex is FileNotFoundException or HttpRequestException;

var fileProcessor = new FileProcessor();
var networkClient = new NetworkClient();

Console.WriteLine("=== Сценарій 1: Читання дампа бази даних (FileProcessor) ===");
try
{
    List<string> dumpLines = RetryHelper.ExecuteWithRetry(
        operation: () => fileProcessor.ReadDatabaseDump("backups/db_dump_2026.sql"),
        retryCount: 3,
        initialDelay: TimeSpan.FromMilliseconds(200),
        shouldRetry: ShouldRetryDatabaseException);

    Console.WriteLine("Результат читання дампа:");
    foreach (string line in dumpLines)
    {
        Console.WriteLine($"  - {line}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Критична помилка сценарію 1: {ex.Message}");
}

Console.WriteLine("\n=== Сценарій 2: Мережевий запит до БД (NetworkClient) ===");
try
{
    List<string> rows = RetryHelper.ExecuteWithRetry(
        operation: () => networkClient.QueryDatabase(
            "Server=db.internal;Database=app_prod;",
            "SELECT id, status, balance FROM accounts;"),
        retryCount: 4,
        initialDelay: TimeSpan.FromMilliseconds(200),
        shouldRetry: ShouldRetryDatabaseException);

    Console.WriteLine("Результат запиту до БД:");
    foreach (string row in rows)
    {
        Console.WriteLine($"  - {row}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Критична помилка сценарію 2: {ex.Message}");
}

Console.WriteLine("\n=== Сценарій 3: Вибірковий shouldRetry (неповторний виняток) ===");
int nonRetryableAttempts = 0;
try
{
    RetryHelper.ExecuteWithRetry<string>(
        operation: () =>
        {
            nonRetryableAttempts++;
            throw new UnauthorizedAccessException("Недостатньо прав доступу до сховища ключів БД.");
        },
        retryCount: 3,
        initialDelay: TimeSpan.FromMilliseconds(200),
        shouldRetry: ShouldRetryDatabaseException);
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Зупинено після {nonRetryableAttempts} спроби без зайвих повторів: {ex.Message}");
}
