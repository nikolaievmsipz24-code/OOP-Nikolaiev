namespace lab18v19;

public class NetworkClient
{
    private int _queryAttempts;

    public List<string> QueryDatabase(string connectionString, string query)
    {
        _queryAttempts++;
        Console.WriteLine($"    [NetworkClient] Виконання запиту '{query}' через '{connectionString}' (#{_queryAttempts})");

        try
        {
            if (_queryAttempts <= 3)
            {
                throw new HttpRequestException(
                    $"Мережевий збій під час звернення до БД за рядком підключення '{connectionString}'.");
            }

            return
            [
                "Row 1: id=101, status=Active, balance=1250.00",
                "Row 2: id=102, status=Pending, balance=430.50"
            ];
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("    [NetworkClient] Перехоплено HttpRequestException у методі QueryDatabase.");
            throw;
        }
        finally
        {
            Console.WriteLine("    [NetworkClient->finally] Очищено стан спроби мережевого запиту.");
        }
    }
}