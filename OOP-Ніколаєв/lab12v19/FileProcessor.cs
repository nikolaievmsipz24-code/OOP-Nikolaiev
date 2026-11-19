namespace lab18v19;

public class FileProcessor
{
    private int _dumpAttempts;

    public List<string> ReadDatabaseDump(string path)
    {
        _dumpAttempts++;
        Console.WriteLine($"    [FileProcessor] Спроба читання дампа '{path}' (#{_dumpAttempts})");

        try
        {
            if (_dumpAttempts <= 2)
            {
                throw new FileNotFoundException(
                    $"Файл дампа БД не знайдено за шляхом '{path}' (тимчасова помилка IO).",
                    path);
            }

            return
            [
                "INSERT INTO users (id, name) VALUES (1, 'Alice');",
                "INSERT INTO users (id, name) VALUES (2, 'Bob');",
                "INSERT INTO roles (id, title) VALUES (1, 'Admin');"
            ];
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("    [FileProcessor] Перехоплено FileNotFoundException у методі ReadDatabaseDump.");
            throw;
        }
        finally
        {
            Console.WriteLine("    [FileProcessor->finally] Завершено ітерацію звернення до файлової системи.");
        }
    }
}
