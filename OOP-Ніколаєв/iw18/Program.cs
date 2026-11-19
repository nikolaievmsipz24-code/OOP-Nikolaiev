using iw18;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine($"Доступно логічних ядер процесора: {Environment.ProcessorCount}\n");

// Порівняння продуктивності LINQ vs PLINQ на 1M, 5M та 10M елементів
foreach (int size in new[] { 1_000_000, 5_000_000, 10_000_000 })
{
    List<int> dataset = ComputeWorkloads.GenerateNumbers(size);
    ComputeWorkloads.CompareLinqVsPlinq(dataset);
}

// Демонстрація побічних ефектів та виправлення на вибірці 2M елементів
List<int> demoData = ComputeWorkloads.GenerateNumbers(2_000_000, seed: 42);
SideEffectsDemo.RunUnsafeSideEffectDemo(demoData, runs: 3);
SideEffectsDemo.RunSafeSynchronizedDemo(demoData);
