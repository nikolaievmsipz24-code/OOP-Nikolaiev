using System.Collections.Generic;
using System.Linq;

namespace lab7v19;

public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new();

    public T? GetById(int id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    public void Add(T item)
    {
        // Перевіряємо, чи немає вже такого ID, або просто додаємо
        _items.Add(item);
    }

    public void Update(T item)
    {
        var existing = GetById(item.Id);
        if (existing != null)
        {
            var index = _items.IndexOf(existing);
            _items[index] = item;
        }
    }

    public void Delete(int id)
    {
        var item = GetById(id);
        if (item != null)
        {
            _items.Remove(item);
        }
    }
}