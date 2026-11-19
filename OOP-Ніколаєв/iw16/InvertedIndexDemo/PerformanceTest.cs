using System.Diagnostics;

namespace InvertedIndexDemo;

public static class PerformanceTest
{
    public static List<Document> GenerateDocuments(List<Document> baseDocuments, int targetCount)
    {
        ArgumentNullException.ThrowIfNull(baseDocuments);
        if (baseDocuments.Count == 0)
        {
            throw new ArgumentException("Базовий список документів не може бути порожнім.", nameof(baseDocuments));
        }

        string[] extraTopics =
        [
            "collections dictionary hashset performance",
            "async tasks linq delegates generics",
            "database indexing search engine query",
            "clean code solid architecture testing"
        ];

        var result = new List<Document>(targetCount);
        for (int i = 0; i < targetCount; i++)
        {
            Document template = baseDocuments[i % baseDocuments.Count];
            string extra = extraTopics[i % extraTopics.Length];

            result.Add(new Document
            {
                Id = i + 1,
                Title = $"{template.Title} #{i + 1}",
                Content = $"{template.Content} Extra context: {extra} batch {i / baseDocuments.Count}."
            });
        }

        return result;
    }

    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        ArgumentNullException.ThrowIfNull(documents);
        ArgumentException.ThrowIfNullOrWhiteSpace(searchWord);

        string normalizedWord = searchWord.Trim().ToLowerInvariant();

        // Побудова індексу поза вимірюванням часу пошуку
        var index = new InvertedIndex();
        foreach (Document doc in documents)
        {
            index.AddDocument(doc);
        }

        // Прогрів (JIT warmup) для уникнення спотворень першого виклику
        _ = documents.Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord)).ToList();
        _ = index.SearchSingleWord(normalizedWord);

        var sw = new Stopwatch();

        // 1. Лінійний пошук
        sw.Restart();
        List<Document> linearResults = documents
            .Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord))
            .ToList();
        sw.Stop();
        long linearTicks = sw.ElapsedTicks;
        double linearMs = sw.Elapsed.TotalMilliseconds;

        // 2. Пошук через Inverted Index
        sw.Restart();
        List<Document> indexResults = index.SearchSingleWord(normalizedWord);
        sw.Stop();
        long indexTicks = sw.ElapsedTicks;
        double indexMs = sw.Elapsed.TotalMilliseconds;

        Console.WriteLine($"--- Документів: {documents.Count} | Слово: '{searchWord}' ---");
        Console.WriteLine($"Linear search : {linearTicks,8} ticks ({linearMs:F4} ms) | Found: {linearResults.Count}");
        Console.WriteLine($"Inverted Index: {indexTicks,8} ticks ({indexMs:F4} ms) | Found: {indexResults.Count}");

        if (linearResults.Count != indexResults.Count)
        {
            Console.WriteLine("УВАГА: Кількість знайдених документів відрізняється!");
        }
    }
}