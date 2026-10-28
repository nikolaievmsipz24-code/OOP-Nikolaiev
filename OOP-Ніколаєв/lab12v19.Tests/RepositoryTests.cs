using Xunit;
using lab12v19;

namespace lab12v19.Tests;

public class RepositoryTests
{
    [Fact]
    public void Add_ShouldAddItemToRepository()
    {
        // Arrange
        var repo = new InMemoryRepository<Event>();
        var ev = new Event { Id = 1, Name = "Test Event", Location = "Rivne", EventDate = System.DateTime.Now };

        // Act
        repo.Add(ev);
        var result = repo.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Event", result.Name);
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenItemDoesNotExist()
    {
        // Arrange
        var repo = new InMemoryRepository<Event>();

        // Act
        var result = repo.GetById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ShouldReturnAllItems()
    {
        // Arrange
        var repo = new InMemoryRepository<Event>();
        repo.Add(new Event { Id = 1, Name = "Event 1" });
        repo.Add(new Event { Id = 2, Name = "Event 2" });

        // Act
        var allItems = repo.GetAll();

        // Assert
        Assert.Collection(allItems,
            item1 => Assert.Equal("Event 1", item1.Name),
            item2 => Assert.Equal("Event 2", item2.Name)
        );
    }

    [Fact]
    public void Update_ShouldModifyExistingItem()
    {
        // Arrange
        var repo = new InMemoryRepository<Event>();
        repo.Add(new Event { Id = 1, Name = "Old Name" });

        // Act
        var updatedEvent = new Event { Id = 1, Name = "New Name" };
        repo.Update(updatedEvent);
        var result = repo.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Name", result.Name);
    }

    [Fact]
    public void Delete_ShouldRemoveItem_WhenItemExists()
    {
        // Arrange
        var repo = new InMemoryRepository<Event>();
        repo.Add(new Event { Id = 1, Name = "To Delete" });

        // Act
        repo.Delete(1);
        var result = repo.GetById(1);

        // Assert
        Assert.Null(result);
    }
}