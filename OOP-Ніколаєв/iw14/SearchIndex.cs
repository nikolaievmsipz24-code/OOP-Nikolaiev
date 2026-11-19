namespace iw14;

public class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> _invertedIndex = new(StringComparer.OrdinalIgnoreCase);

    public void AddDocument(int docId, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        string[] words = content
            .Split([' ', '\t', '\r', '\n', ',', '.', '!', '?', ':', ';', '-', '(', ')', '"', '\''],
                StringSplitOptions.RemoveEmptyEntries);

        foreach (string rawWord in words)
        {
            string word = rawWord.ToLowerInvariant();

            if (!_invertedIndex.TryGetValue(word, out HashSet<int>? docIds))
            {
                docIds = new HashSet<int>();
                _invertedIndex[word] = docIds;
            }

            docIds.Add(docId);
        }
    }

    public HashSet<int> Search(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return [];
        }

        if (_invertedIndex.TryGetValue(word.Trim().ToLowerInvariant(), out HashSet<int>? docIds))
        {
            return new HashSet<int>(docIds);
        }

        return [];
    }

    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0)
        {
            return [];
        }

        List<HashSet<int>> matchingSets = [];

        foreach (string word in words)
        {
            HashSet<int> found = Search(word);
            if (found.Count == 0)
            {
                return [];
            }

            matchingSets.Add(found);
        }

        var result = new HashSet<int>(matchingSets[0]);
        for (int i = 1; i < matchingSets.Count; i++)
        {
            result.IntersectWith(matchingSets[i]);
        }

        return result;
    }
}