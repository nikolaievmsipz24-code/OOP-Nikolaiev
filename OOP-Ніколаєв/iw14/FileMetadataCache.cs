namespace iw14;

public class FileMetadata
{
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = string.Empty;

    public override string ToString() =>
        $"{FileName} ({Size} B, modified: {LastModified:yyyy-MM-dd HH:mm}, hash: {Hash})";
}

public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache;

    public FileMetadataCache(IEqualityComparer<string>? comparer = null)
    {
        _cache = new Dictionary<string, FileMetadata>(comparer ?? StringComparer.OrdinalIgnoreCase);
    }

    public int Count => _cache.Count;

    public void Add(string filePath, FileMetadata metadata)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(metadata);

        _cache[filePath] = metadata;
    }

    public bool TryGet(string filePath, out FileMetadata? metadata)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            metadata = null;
            return false;
        }

        return _cache.TryGetValue(filePath, out metadata);
    }

    public FileMetadata Get(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!_cache.TryGetValue(filePath, out FileMetadata? metadata))
        {
            throw new KeyNotFoundException($"Метадані для файлу '{filePath}' відсутні в кеші.");
        }

        return metadata;
    }

    public bool Remove(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        return _cache.Remove(filePath);
    }

    public bool Contains(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        return _cache.ContainsKey(filePath);
    }
}