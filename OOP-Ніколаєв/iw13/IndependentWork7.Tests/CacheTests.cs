using IndependentWork7;

namespace IndependentWork7.Tests;

public class CacheTests
{
    [Fact]
    public void Add_And_Get_ShouldStoreAndReturnValues()
    {
        var cache = new Cache<string, int>(3);

        cache.Add("one", 1);
        cache.Add("two", 2);

        Assert.Equal(1, cache.Get("one"));
        Assert.Equal(2, cache.Get("two"));
    }

    [Fact]
    public void Add_WhenCapacityExceeded_ShouldEvictOldestItemByFifo()
    {
        var cache = new Cache<string, string>(2);

        cache.Add("k1", "v1");
        cache.Add("k2", "v2");
        cache.Add("k3", "v3"); // має витіснити "k1"

        Assert.False(cache.ContainsKey("k1"));
        Assert.True(cache.ContainsKey("k2"));
        Assert.True(cache.ContainsKey("k3"));
        Assert.Equal("v2", cache.Get("k2"));
        Assert.Equal("v3", cache.Get("k3"));
        Assert.Throws<KeyNotFoundException>(() => cache.Get("k1"));
    }

    [Fact]
    public void Get_NonExistentKey_ShouldThrowKeyNotFoundException()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "First");

        Assert.Throws<KeyNotFoundException>(() => cache.Get(999));
    }

    [Fact]
    public void ContainsKey_ShouldReturnTrueForExistingAndFalseForMissing()
    {
        var cache = new Cache<string, string>(2);
        cache.Add("existing", "value");

        Assert.True(cache.ContainsKey("existing"));
        Assert.False(cache.ContainsKey("missing"));
    }

    [Fact]
    public void Count_ShouldTrackCurrentItemsAndNotExceedCapacity()
    {
        var cache = new Cache<int, string>(2);

        Assert.Equal(0, cache.Count);

        cache.Add(1, "A");
        Assert.Equal(1, cache.Count);

        cache.Add(2, "B");
        Assert.Equal(2, cache.Count);

        cache.Add(3, "C");
        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void Add_ExistingKey_ShouldUpdateValueWithoutIncreasingCount()
    {
        var cache = new Cache<string, int>(2);
        cache.Add("score", 10);
        cache.Add("score", 25);

        Assert.Equal(1, cache.Count);
        Assert.Equal(25, cache.Get("score"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Constructor_InvalidCapacity_ShouldThrowArgumentOutOfRangeException(int invalidCapacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, string>(invalidCapacity));
    }
}