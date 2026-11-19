namespace iw20;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new();

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxRetries), "Кількість повторних спроб не може бути від'ємною.");
        }

        if (initialDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(initialDelay), "Початкова затримка не може бути від'ємною.");
        }

        if (jitterFactor < 0 || jitterFactor >= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(jitterFactor), "Коефіцієнт jitter має бути в межах [0, 1).");
        }

        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        Func<Exception, bool> retryPredicate = shouldRetry ?? (_ => true);
        int retryAttempt = 0;

        while (true)
        {
            int attemptNumber = retryAttempt + 1;

            try
            {
                Console.WriteLine($"[Спроба {attemptNumber}] Виконую операцію...");
                T result = operation();
                Console.WriteLine($"[Спроба {attemptNumber}] Успішно.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Спроба {attemptNumber}] Невдача: {ex.GetType().Name} — {ex.Message}");

                bool canRetry = retryAttempt < _maxRetries && retryPredicate(ex);
                if (!canRetry)
                {
                    Console.WriteLine($"[RetryPolicy] Повторні спроби припинено (використано спроб: {attemptNumber}, ліміт повторів: {_maxRetries}). Викидаю виняток.");
                    throw;
                }

                double exponentialMultiplier = Math.Pow(2, retryAttempt);
                double jitterMultiplier = 1.0 + _random.NextDouble() * _jitterFactor * 2.0 - _jitterFactor;
                double delayMilliseconds = _initialDelay.TotalMilliseconds * exponentialMultiplier * jitterMultiplier;

                TimeSpan delay = delayMilliseconds > 0
                    ? TimeSpan.FromMilliseconds(delayMilliseconds)
                    : TimeSpan.Zero;

                Console.WriteLine(
                    $"[RetryPolicy] Очікування {delay.TotalMilliseconds:F1} мс перед повторною спробою #{retryAttempt + 2} (retryAttempt={retryAttempt}, jitterFactor={_jitterFactor})...");

                if (delay > TimeSpan.Zero)
                {
                    Thread.Sleep(delay);
                }

                retryAttempt++;
            }
        }
    }
}