using InvertedIndexDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

List<Document> baseDocuments =
[
    new()
    {
        Id = 1,
        Title = "Introduction to C#",
        Content = "C# is a modern object-oriented programming language developed by Microsoft."
    },
    new()
    {
        Id = 2,
        Title = "Java Basics",
        Content = "Java is a popular programming language known for its portability and performance."
    },
    new()
    {
        Id = 3,
        Title = "Python Overview",
        Content = "Python is a high-level programming language with simple syntax."
    },
    new()
    {
        Id = 4,
        Title = "C# Advanced Features",
        Content = "C# supports LINQ, async/await, and many modern programming paradigms."
    },
    new()
    {
        Id = 5,
        Title = "Comparing Languages",
        Content = "C#, Java, and Python are among the most popular programming languages today."
    }
];

var mainIndex = new InvertedIndex();
foreach (Document doc in baseDocuments)
{
    mainIndex.AddDocument(doc);
}

Console.WriteLine($"Loaded {mainIndex.DocumentCount} documents");
Console.WriteLine($"Index contains {mainIndex.UniqueWords} unique words\n");

Console.WriteLine("=== Пошук одного слова ('c#') ===");
foreach (Document doc in mainIndex.SearchSingleWord("c#"))
{
    Console.WriteLine($"  {doc}");
}

Console.WriteLine("\n=== Пошук за ВСІМА словами AND ('programming', 'language', 'python') ===");
foreach (Document doc in mainIndex.SearchMultipleWords("programming", "language", "python"))
{
    Console.WriteLine($"  {doc}");
}

Console.WriteLine("\n=== Пошук за БУДЬ-ЯКИМ словом OR ('async', 'portability') ===");
foreach (Document doc in mainIndex.SearchAnyWord("async", "portability"))
{
    Console.WriteLine($"  {doc}");
}

Console.WriteLine("\n=== Вимірювання продуктивності ===");
foreach (int size in new[] { 10, 100, 1000, 10000 })
{
    List<Document> dataset = PerformanceTest.GenerateDocuments(baseDocuments, size);
    PerformanceTest.CompareSearchMethods(dataset, "programming");
}
