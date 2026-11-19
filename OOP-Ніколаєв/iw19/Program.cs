using iw19;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Демонстрація власного винятку InsufficientFundsException ===");
decimal balance = 1200.00m;
decimal requested = 1850.00m;

try
{
    Console.WriteLine($"Поточний баланс: {balance:F2} грн. Запит на списання: {requested:F2} грн.");

    if (requested > balance)
    {
        throw new InsufficientFundsException(
            "Недостатньо коштів на рахунку для проведення списання.",
            requested,
            balance);
    }

    balance -= requested;
    Console.WriteLine($"Списання успішне. Новий баланс: {balance:F2} грн.");
}
catch (InsufficientFundsException ex)
{
    Console.WriteLine($"Перехоплено InsufficientFundsException: {ex.Message}");
    Console.WriteLine($"  Запитана сума : {ex.RequestedAmount:F2} грн");
    Console.WriteLine($"  Поточний баланс: {ex.CurrentBalance:F2} грн");
    Console.WriteLine($"  Не вистачає   : {ex.RequestedAmount - ex.CurrentBalance:F2} грн");
}
catch (Exception ex)
{
    Console.WriteLine($"Інша помилка: {ex.Message}");
}
finally
{
    Console.WriteLine("Блок finally: завершено перевірку балансу.\n");
}

ResilienceScenarios.RunPaymentGatewayRetryScenario();
ResilienceScenarios.RunAnalyticsCircuitBreakerScenario();
ResilienceScenarios.RunQueueSyncRetryWithTimeoutScenario();
