using System.Collections.Concurrent;

namespace iw18;

public static class SideEffectsDemo
{
    public static void RunUnsafeSideEffectDemo(List<int> data, int runs = 3)
    {
        Console.WriteLine("=== Небезпечний сценарій PLINQ: побічні ефекти без синхронізації ===");

        for (int run = 1; run <= runs; run++)
        {
            int unsafeProcessedCount = 0;
            var unsafeResults = new List<double>(); // List<T> не потокобезпечний при паралельному Add

            try
            {
                _ = data
                    .AsParallel()
                    .Where(ComputeWorkloads.IsPrime)
                    .Select(n =>
                    {
                        unsafeProcessedCount++; // Race condition: читання-модифікація-запис без синхронізації
                        double val = ComputeWorkloads.HeavyTransform(n);
                        unsafeResults.Add(val); // Можливе пошкодження внутрішнього стану List<T> або втрата елементів
                        return val;
                    })
                    .ToList();

                Console.WriteLine($"  Запуск #{run}: лічильник = {unsafeProcessedCount:N0}, елементів у List = {unsafeResults.Count:N0} (очікувано більше і стабільно)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Запуск #{run}: виняток через паралельний доступ до List<T> -> {ex.GetType().Name}: {ex.Message}");
            }
        }

        Console.WriteLine();
    }

    public static void RunSafeSynchronizedDemo(List<int> data)
    {
        Console.WriteLine("=== Виправлений сценарій PLINQ: Interlocked + ConcurrentBag<T> ===");

        int safeProcessedCount = 0;
        var safeBag = new ConcurrentBag<double>();
        object syncLock = new();
        int lockedCount = 0;

        _ = data
            .AsParallel()
            .Where(ComputeWorkloads.IsPrime)
            .Select(n =>
            {
                Interlocked.Increment(ref safeProcessedCount);
                double val = ComputeWorkloads.HeavyTransform(n);
                safeBag.Add(val);

                lock (syncLock)
                {
                    lockedCount++;
                }

                return val;
            })
            .ToList();

        Console.WriteLine($"  Interlocked лічильник : {safeProcessedCount:N0}");
        Console.WriteLine($"  lock лічильник        : {lockedCount:N0}");
        Console.WriteLine($"  ConcurrentBag елементів: {safeBag.Count:N0}");
        Console.WriteLine();
    }
}