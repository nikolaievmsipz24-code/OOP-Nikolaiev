namespace iw14;

public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags;

    public TagManager()
    {
        _articleTags = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
    }

    public void AddTag(string articleId, string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(articleId);
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        string normalizedTag = tag.Trim().ToLowerInvariant();

        if (!_articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _articleTags[articleId] = tags;
        }

        tags.Add(normalizedTag);
    }

    public bool RemoveTag(string articleId, string tag)
    {
        if (string.IsNullOrWhiteSpace(articleId) || string.IsNullOrWhiteSpace(tag))
        {
            return false;
        }

        if (!_articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            return false;
        }

        return tags.Remove(tag.Trim().ToLowerInvariant());
    }

    public IReadOnlyCollection<string> GetArticleTags(string articleId)
    {
        if (!string.IsNullOrWhiteSpace(articleId) && _articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            return tags;
        }

        return Array.Empty<string>();
    }

    public HashSet<string> GetAllUniqueTags()
    {
        var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (HashSet<string> tags in _articleTags.Values)
        {
            allTags.UnionWith(tags);
        }

        return allTags;
    }

    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        if (!_articleTags.TryGetValue(articleId1, out HashSet<string>? tags1) ||
            !_articleTags.TryGetValue(articleId2, out HashSet<string>? tags2))
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        var common = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
        common.IntersectWith(tags2);
        return common;
    }

    public HashSet<string> FindExclusiveTags(string primaryArticleId, string otherArticleId)
    {
        if (!_articleTags.TryGetValue(primaryArticleId, out HashSet<string>? primaryTags))
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        var exclusive = new HashSet<string>(primaryTags, StringComparer.OrdinalIgnoreCase);

        if (_articleTags.TryGetValue(otherArticleId, out HashSet<string>? otherTags))
        {
            exclusive.ExceptWith(otherTags);
        }

        return exclusive;
    }
}