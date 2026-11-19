using System.Diagnostics;

namespace iw17;

public static class AnalyticsPipelines
{
    public static void TopProductsByCategory(List<Sale> sales, List<Product> products)
    {
        var sw = Stopwatch.StartNew();

        var groupedByCategory = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalQuantity = g.Sum(s => s.Quantity)
            })
            .Join(
                products,
                sold => sold.ProductId,
                p => p.Id,
                (sold, p) => new
                {
                    p.Category,
                    p.Name,
                    sold.TotalQuantity
                })
            .GroupBy(x => x.Category)
            .OrderBy(cg => cg.Key)
            .Select(cg => new
            {
                Category = cg.Key,
                TopProducts = cg
                    .OrderByDescending(item => item.TotalQuantity)
                    .ThenBy(item => item.Name)
                    .Take(3)
                    .ToList()
            })
            .ToList();

        sw.Stop();

        Console.WriteLine($"=== Пайплайн 1: Топ-3 продукти за категоріями (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks) ===");
        foreach (var group in groupedByCategory)
        {
            Console.WriteLine($"Категорія: {group.Category}");
            foreach (var item in group.TopProducts)
            {
                Console.WriteLine($"  - {item.Name}: {item.TotalQuantity} шт.");
            }
        }
        Console.WriteLine();
    }

    public static void RevenueByCustomerCity(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var sw = Stopwatch.StartNew();

        var cityRevenues = sales
            .Join(
                products,
                s => s.ProductId,
                p => p.Id,
                (s, p) => new
                {
                    s.CustomerId,
                    Revenue = s.Quantity * p.UnitPrice
                })
            .Join(
                customers,
                sp => sp.CustomerId,
                c => c.Id,
                (sp, c) => new
                {
                    c.City,
                    sp.Revenue
                })
            .GroupBy(x => x.City)
            .Select(g => new
            {
                City = g.Key,
                TotalRevenue = g.Sum(x => x.Revenue),
                TransactionsCount = g.Count()
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        sw.Stop();

        Console.WriteLine($"=== Пайплайн 2: Загальний дохід за містами клієнтів (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks) ===");
        foreach (var item in cityRevenues)
        {
            Console.WriteLine($"  - {item.City,-12}: {item.TotalRevenue,12:F2} грн (транзакцій: {item.TransactionsCount})");
        }
        Console.WriteLine();
    }

    public static void SalesByYearAndMonth(List<Sale> sales, List<Product> products)
    {
        var sw = Stopwatch.StartNew();

        var monthlyStats = sales
            .Join(
                products,
                s => s.ProductId,
                p => p.Id,
                (s, p) => new
                {
                    s.SaleDate.Year,
                    s.SaleDate.Month,
                    s.Quantity,
                    Revenue = s.Quantity * p.UnitPrice
                })
            .GroupBy(x => new { x.Year, x.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalUnits = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        sw.Stop();

        Console.WriteLine($"=== Пайплайн 3: Продажі за місяцями та роками (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks) ===");
        foreach (var row in monthlyStats)
        {
            Console.WriteLine($"  - {row.Year}-{row.Month:D2}: {row.TotalUnits,3} шт. | Дохід: {row.TotalRevenue,12:F2} грн");
        }
        Console.WriteLine();
    }

    public static void TopCustomersBySpending(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var sw = Stopwatch.StartNew();

        var topCustomers = sales
            .Join(
                products,
                s => s.ProductId,
                p => p.Id,
                (s, p) => new
                {
                    s.CustomerId,
                    Amount = s.Quantity * p.UnitPrice
                })
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                TotalSpent = g.Sum(x => x.Amount)
            })
            .Where(x => x.TotalSpent > 0m)
            .Join(
                customers,
                spend => spend.CustomerId,
                c => c.Id,
                (spend, c) => new
                {
                    c.Name,
                    c.City,
                    spend.TotalSpent
                })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .ToList();

        sw.Stop();

        Console.WriteLine($"=== Пайплайн 4 (додатковий): Топ-5 клієнтів за сумою покупок (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks) ===");
        foreach (var c in topCustomers)
        {
            Console.WriteLine($"  - {c.Name} ({c.City}): {c.TotalSpent:F2} грн");
        }
        Console.WriteLine();
    }
}