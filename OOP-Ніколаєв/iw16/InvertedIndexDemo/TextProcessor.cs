using System.Text.RegularExpressions;

namespace InvertedIndexDemo;

public static partial class TextProcessor
{
    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    [GeneratedRegex(@"[\p{L}\p{N}#\+]+", RegexOptions.Compiled)]
    private static partial Regex WordTokenRegex();

    public static IEnumerable<string> Tokenize(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        string lower = text.ToLowerInvariant();
        return WordTokenRegex()
            .Matches(lower)
            .Select(m => m.Value)
            .Where(w => !string.IsNullOrWhiteSpace(w));
    }

    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);
        return words.Where(w => !StopWords.Contains(w));
    }
}