using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace iw19;

public static class ResilienceScenarios
{
    // Сценарій 1: Зовнішній API з Retry + Exponential Backoff + Jitter
    public static void RunPaymentGatewayRetryScenario()
    {
        Console.WriteLine("=== Сценарій 1: Зовнішній платіжний API (Retry + Exponential Backoff + Jitter) ===");

        int attempt = 0;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(200),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    Console.WriteLine(
                        $"  [Retry #{args.AttemptNumber + 1}] Пауза {args.RetryDelay.TotalMilliseconds:F0} мс через помилку: {args.Outcome.Exception?.Message}");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        try
        {
            string receipt = pipeline.Execute(_ =>
            {
                attempt++;
                Console.WriteLine($"  [PaymentApi] Виклик шлюзу оплати (спроба {attempt})...");

                if (attempt <= 2)
                {
                    throw new HttpRequestException("Тимчасовий таймаут відповіді від платіжного шлюзу (503/Network).");
                }

                return "TXN-2026-99481";
            });

            Console.WriteLine($"  [Результат] Оплату успішно підтверджено. Код транзакції: {receipt}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [Результат] Операція завершилась остаточною помилкою: {ex.Message}\n");
        }
    }

    // Сценарій 2: Сервіс звітів під тривалим збоєм -> Circuit Breaker
    public static void RunAnalyticsCircuitBreakerScenario()
    {
        Console.WriteLine("=== Сценарій 2: Сервіс звітів (Circuit Breaker: Closed -> Open) ===");

        var circuitBreaker = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<TimeoutException>().Handle<HttpRequestException>(),
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 2,
                BreakDuration = TimeSpan.FromSeconds(2),
                OnOpened = args =>
                {
                    Console.WriteLine($"  [CircuitBreaker -> OPEN] Ланцюг розімкнено на {args.BreakDuration.TotalSeconds} с через: {args.Outcome.Exception?.Message}");
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    Console.WriteLine("  [CircuitBreaker -> CLOSED] Ланцюг замкнено, роботу відновлено.");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = _ =>
                {
                    Console.WriteLine("  [CircuitBreaker -> HALF-OPEN] Тестовий виклик для перевірки сервісу.");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        for (int call = 1; call <= 4; call++)
        {
            try
            {
                Console.WriteLine($"  [AnalyticsClient] Запит #{call} до сервісу звітів...");
                circuitBreaker.Execute(_ =>
                {
                    throw new HttpRequestException("Сервіс звітів недоступний (connection refused).");
                });
            }
            catch (BrokenCircuitException ex)
            {
                Console.WriteLine($"  [Результат #{call}] Запит миттєво відхилено Circuit Breaker без звернення до сервісу: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [Результат #{call}] Помилка виклику: {ex.Message}");
            }
        }

        Console.WriteLine();
    }

    // Сценарій 3: Черга повідомлень / повільний запит -> Комбінація Retry + Timeout
    public static void RunQueueSyncRetryWithTimeoutScenario()
    {
        Console.WriteLine("=== Сценарій 3: Синхронізація черги повідомлень (Retry + Timeout) ===");

        int attempt = 0;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<TimeoutRejectedException>().Handle<IOException>(),
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromMilliseconds(150),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    Console.WriteLine(
                        $"  [Retry #{args.AttemptNumber + 1}] Повтор після {args.Outcome.Exception?.GetType().Name} (затримка {args.RetryDelay.TotalMilliseconds:F0} мс)");
                    return ValueTask.CompletedTask;
                }
            })
            .AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromMilliseconds(200),
                OnTimeout = args =>
                {
                    Console.WriteLine($"  [Timeout] Перевищено ліміт часу {args.Timeout.TotalMilliseconds:F0} мс.");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        try
        {
            string status = pipeline.Execute(token =>
            {
                attempt++;
                Console.WriteLine($"  [QueueWorker] Обробка пакета з черги (спроба {attempt})...");

                if (attempt == 1)
                {
                    // Імітуємо зависання довше за Timeout (200 мс)
                    Thread.Sleep(350);
                }

                token.ThrowIfCancellationRequested();
                return "BATCH-PROCESSED-OK";
            });

            Console.WriteLine($"  [Результат] Пакет успішно оброблено: {status}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  [Результат] Пакет не оброблено: {ex.GetType().Name} — {ex.Message}\n");
        }
    }
}