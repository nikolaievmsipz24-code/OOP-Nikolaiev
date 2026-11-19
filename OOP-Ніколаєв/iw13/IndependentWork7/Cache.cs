namespace IndependentWork7;

public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly Queue<TKey> _keysOrder;

    public Cache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Ємність кешу має бути більшою за 0.");
        }

        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>(capacity);
        _keysOrder = new Queue<TKey>(capacity);
    }

    public int Count => _cache.Count;

    public void Add(TKey key, TValue value)
    {
        ArgumentNullException.ThrowIfNull(key);

        // Якщо ключ уже є — просто оновлюємо значення
        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        // Якщо кеш заповнений — видаляємо найстаріший елемент (FIFO)
        if (_cache.Count >= _capacity)
        {
            TKey oldestKey = _keysOrder.Dequeue();
            _cache.Remove(oldestKey);
        }

        _cache[key] = value;
        _keysOrder.Enqueue(key);
    }

    public TValue Get(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (!_cache.TryGetValue(key, out TValue? value))
        {
            throw new KeyNotFoundException($"Ключ '{key}' не знайдено в кеші.");
        }

        return value;
    }

    public bool ContainsKey(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _cache.ContainsKey(key);
    }
}