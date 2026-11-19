namespace InvertedIndexDemo;

public class InvertedIndex
{
    private readonly Dictionary<string, HashSet<int>> _index;
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);
        _documents = new Dictionary<int, Document>();
    }

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;

    public void AddDocument(Document doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        _documents[doc.Id] = doc;

        foreach (string word in TextProcessor.Tokenize(doc.Content))
        {
            if (!_index.TryGetValue(word, out HashSet<int>? docIds))
            {
                docIds = new HashSet<int>();
                _index[word] = docIds;
            }

            docIds.Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return [];
        }

        string normalized = word.Trim().ToLowerInvariant();
        if (!_index.TryGetValue(normalized, out HashSet<int>? docIds) || docIds.Count == 0)
        {
            return [];
        }

        return ResolveDocuments(docIds);
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        if (words is null || words.Length == 0)
        {
            return [];
        }

        List<string> normalizedWords = words
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Select(w => w.Trim().ToLowerInvariant())
            .Distinct()
            .ToList();

        if (normalizedWords.Count == 0)
        {
            return [];
        }

        HashSet<int>? resultIds = null;

        foreach (string word in normalizedWords)
        {
            if (!_index.TryGetValue(word, out HashSet<int>? wordDocs) || wordDocs.Count == 0)
            {
                return [];
            }

            if (resultIds is null)
            {
                resultIds = new HashSet<int>(wordDocs);
            }
            else
            {
                resultIds.IntersectWith(wordDocs);
                if (resultIds.Count == 0)
                {
                    return [];
                }
            }
        }

        return resultIds is null ? [] : ResolveDocuments(resultIds);
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        if (words is null || words.Length == 0)
        {
            return [];
        }

        var unionIds = new HashSet<int>();

        foreach (string rawWord in words)
        {
            if (string.IsNullOrWhiteSpace(rawWord))
            {
                continue;
            }

            string normalized = rawWord.Trim().ToLowerInvariant();
            if (_index.TryGetValue(normalized, out HashSet<int>? wordDocs))
            {
                unionIds.UnionWith(wordDocs);
            }
        }

        return unionIds.Count == 0 ? [] : ResolveDocuments(unionIds);
    }

    private List<Document> ResolveDocuments(IEnumerable<int> docIds)
    {
        var result = new List<Document>();
        foreach (int id in docIds.OrderBy(i => i))
        {
            if (_documents.TryGetValue(id, out Document? doc))
            {
                result.Add(doc);
            }
        }

        return result;
    }
}