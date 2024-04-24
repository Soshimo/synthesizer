using System.Collections.Concurrent;

namespace SynthesizerUI.Services;

public interface IObjectFactory<out T>
{
    T CreateInstance();
}

public class ObjectPool<T, TFactory>
    where T : IPoolObject
    where TFactory : IObjectFactory<T>
{
    private ConcurrentBag<T> _pool = new ConcurrentBag<T>();

    private int _count;
    private const int GrowthFactor = 2; // Exponential growth factor - 1 * 2 = 2, 2 * 2 = 4, 4 * 2 = 8, 8 * 2 = 16 ...

    private TFactory _factory;

    public ObjectPool(TFactory factory, int initialCapacity = 1)
    {
        _factory = factory;
        _count = initialCapacity;
        GrowPool(initialCapacity);
    }

    public T? GetObject()
    {
        while (!_pool.IsEmpty)
        {
            if (!_pool.TryTake(out var item)) continue;

            if (item.IsComplete)
            {
                return item;
            }
            else
            {
                // Optionally, you can keep a count of checked-out items
                // and only return them to the pool if they exceed a certain threshold.
                // This example simply discards the reference, forcing growth if needed.
            }
        }

        // Grow the pool if no complete object is found
        GrowPoolIfNeeded();
        return GetObject(); // Recursively attempt to get a new object

    }

    private void GrowPoolIfNeeded()
    {
        if (_pool.IsEmpty) // Check if pool is empty or add more condition as needed
        {
            var newCount = Math.Max(_count * GrowthFactor, 1); // Ensure at least one object is added
            GrowPool(newCount);
            _count += newCount;
        }
    }

    public void ReturnObject(T item)
    {
        item.Reset();
        _pool.Add(item);
    }

    private void GrowPool(int count)
    {
        for (var i = 0; i < count; i++)
        {
            _pool.Add(_factory.CreateInstance());
        }
    }
}