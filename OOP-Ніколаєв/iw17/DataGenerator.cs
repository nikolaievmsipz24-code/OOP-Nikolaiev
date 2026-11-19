namespace iw17;

public static class DataGenerator
{
    private const int Seed = 20261119;

    public static (List<Product> Products, List<Customer> Customers, List<Sale> Sales) GenerateDataset()
    {
        var random = new Random(Seed);

        string[] categories = ["Laptops", "Peripherals", "Monitors", "Audio", "Networking"];
        string[] productPrefixes = ["Pro", "Air", "Ultra", "Compact", "Studio", "Gaming", "Essential", "Max"];
        string[] cities = ["Kyiv", "Rivne", "Lviv", "Kharkiv", "Odesa", "Dnipro", "Vinnytsia", "Ternopil"];
        string[] firstNames = ["Maksym", "Emilia", "Danylo", "Oksana", "Yurii", "Sofiia", "Artem", "Kateryna", "Illia", "Diana"];
        string[] lastNames = ["Koval", "Shevchenko", "Bondar", "Tkachenko", "Kravets", "Melnyk", "Marchenko", "Polischuk"];

        var products = new List<Product>(60);
        for (int i = 1; i <= 60; i++)
        {
            string category = categories[(i - 1) % categories.Length];
            string prefix = productPrefixes[random.Next(productPrefixes.Length)];
            decimal price = Math.Round((decimal)(random.Next(300, 35000) + random.NextDouble()), 2);

            products.Add(new Product
            {
                Id = i,
                Name = $"{prefix} {category[..^1]} #{i}",
                Category = category,
                UnitPrice = price
            });
        }

        var customers = new List<Customer>(80);
        for (int i = 1; i <= 80; i++)
        {
            string first = firstNames[random.Next(firstNames.Length)];
            string last = lastNames[random.Next(lastNames.Length)];
            string city = cities[random.Next(cities.Length)];

            customers.Add(new Customer
            {
                Id = i,
                Name = $"{first} {last}",
                City = city
            });
        }

        var sales = new List<Sale>(120);
        DateTime startDate = new(2025, 1, 1);
        int totalDays = 680;

        for (int i = 1; i <= 120; i++)
        {
            int productId = random.Next(1, products.Count + 1);
            int customerId = random.Next(1, customers.Count + 1);
            int quantity = random.Next(1, 8);
            DateTime saleDate = startDate.AddDays(random.Next(0, totalDays));

            sales.Add(new Sale
            {
                Id = i,
                ProductId = productId,
                Quantity = quantity,
                SaleDate = saleDate,
                CustomerId = customerId
            });
        }

        return (products, customers, sales);
    }
}