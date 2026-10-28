using System;
using lab12v19;

class Program
{
    static void Main(string[] args)
    {
        IRepository<Event> repository = new InMemoryRepository<Event>();

        repository.Add(new Event { Id = 1, Name = "C# Meetup", Location = "Rivne IT College", EventDate = new DateTime(2026, 11, 10) });
        repository.Add(new Event { Id = 2, Name = "Hackathon", Location = "Online", EventDate = new DateTime(2026, 12, 1) });

        Console.WriteLine("All events:");
        foreach (var ev in repository.GetAll())
        {
            Console.WriteLine($"- [{ev.Id}] {ev.Name} at {ev.Location} ({ev.EventDate.ToShortDateString()})");
        }
    }
}
