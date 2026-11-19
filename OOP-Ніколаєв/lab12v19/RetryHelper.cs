namespace lab18v19;

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        if (retryCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(retryCount), "Кількість повторів не може бути від'ємною.");
        }

        TimeSpan baseDelay = initialDelay == default ? TimeSpan.FromMilliseconds(200) : initialDelay;
        Func<Exception, bool> retryPredicate = shouldRetry ?? DefaultShouldRetry;

        int attempt = 0;

        while (true)
        {
            attempt++;
            try
            {
                Console.WriteLine($"[RetryHelper] Виконання спроби {attempt} (макс. повторів після збою: {retryCount})...");
                T result = operation();
                Console.WriteLine($"[RetryHelper] Спроба {attempt} завершилась успішно.");
                return result;
            }
            catch (Exception ex)
            {
                int retryIndex = attempt - 1; // 0 для першої повторної спроби, 1 для другої тощо
                bool canRetry = retryIndex < retryCount && retryPredicate(ex);

                Console.WriteLine($"[RetryHelper] Помилка на спробі {attempt} ({ex.GetType().Name}): {ex.Message}");

                if (!canRetry)
                {
                    Console.WriteLine("[RetryHelper] Повторні спроби не виконуватимуться. Прокидаємо виняток далі.");
                    throw;
                }

                double multiplier = Math.Pow(2, retryIndex);
                TimeSpan delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * multiplier);

                Console.WriteLine($"[RetryHelper] Очікування {delay.TotalMilliseconds:0} мс перед спробою {attempt + 1} (множник 2^{retryIndex} = {multiplier})...");
                Thread.Sleep(delay);
            }
        }
    }

    private static bool DefaultShouldRetry(Exception ex) =>
        ex is IOException or HttpRequestException;
}