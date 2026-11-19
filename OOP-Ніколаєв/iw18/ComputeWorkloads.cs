using System.Diagnostics;

namespace iw18;

public static class ComputeWorkloads
{
    public static List<int> GenerateNumbers(int count, int seed = 20261119)
    {
        var random = new Random(seed);
        var numbers = new List<int>(count);

        for (int i = 0; i < count; i++)
        {
            // Діапазон чисел, де IsPrime виконує достатньо ітерацій, але без зайвої вічності
            numbers.Add(random.Next(100_000, 2_000_000));
        }

        return numbers;
    }

    public static bool IsPrime(int n)
    {
        if (n < 2)
        {
            return false;
        }

        if (n % 2 == 0)
        {
            return n == 2;
        }

        int limit = (int)Math.Sqrt(n);
        for (int divisor = 3; divisor <= limit; divisor += 2)
        {
            if (n % divisor == 0)
            {
                return false;
            }
        }

        return true;
    }

    public static double HeavyTransform(int prime)
    {
        double acc = 0;
        for (int i = 1; i <= 25; i++)
        {
            acc += Math.Sqrt(prime + i) * Math.Sin(prime % 97 + i) + Math.Log(prime + i * 10.0);
        }

        return Math.Round(acc, 4);
    }

    public static void CompareLinqVsPlinq(List<int> data)
    {
        // Прогрів (JIT warmup)
        _ = data.Where(IsPrime).Select(HeavyTransform).Take(1).ToList();
        _ = data.AsParallel().Where(IsPrime).Select(HeavyTransform).Take(1).ToList();

        var swLinq = Stopwatch.StartNew();
        List<double> linqResult = data
            .Where(IsPrime)
            .Select(HeavyTransform)
            .ToList();
        swLinq.Stop();

        var swPlinq = Stopwatch.StartNew();
        List<double> plinqResult = data
            .AsParallel()
            .Where(IsPrime)
            .Select(HeavyTransform)
            .ToList();
        swPlinq.Stop();

        double speedup = swPlinq.Elapsed.TotalMilliseconds > 0
            ? swLinq.Elapsed.TotalMilliseconds / swPlinq.Elapsed.TotalMilliseconds
            : 0;

        Console.WriteLine($"Розмір колекції: {data.Count:N0} елементів");
        Console.WriteLine($"  LINQ  : {swLinq.Elapsed.TotalMilliseconds,8:F2} мс ({swLinq.ElapsedTicks,11:N0} ticks) | знайдено: {linqResult.Count:N0}");
        Console.WriteLine($"  PLINQ : {swPlinq.Elapsed.TotalMilliseconds,8:F2} мс ({swPlinq.ElapsedTicks,11:N0} ticks) | знайдено: {plinqResult.Count:N0}");
        Console.WriteLine($"  Співвідношення часу (LINQ / PLINQ): {speedup:F2}x\n");
    }
}