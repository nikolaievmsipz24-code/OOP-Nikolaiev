using iw17;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var (products, customers, sales) = DataGenerator.GenerateDataset();

Console.WriteLine($"Згенеровано даних: {products.Count} товарів, {customers.Count} клієнтів, {sales.Count} продажів.\n");

AnalyticsPipelines.TopProductsByCategory(sales, products);
AnalyticsPipelines.RevenueByCustomerCity(sales, products, customers);
AnalyticsPipelines.SalesByYearAndMonth(sales, products);
AnalyticsPipelines.TopCustomersBySpending(sales, products, customers);
