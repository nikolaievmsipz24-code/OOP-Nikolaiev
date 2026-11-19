using iw14;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Сценарій 1: FileMetadataCache (Dictionary) ===");
var cache = new FileMetadataCache();
cache.Add("configs/appsettings.json", new FileMetadata
{
    FileName = "appsettings.json",
    Size = 2048,
    LastModified = DateTime.UtcNow.AddMinutes(-15),
    Hash = "9f86d081884c7d65"
});
cache.Add("logs/app.log", new FileMetadata
{
    FileName = "app.log",
    Size = 65536,
    LastModified = DateTime.UtcNow,
    Hash = "4b227777d4dd1fc6"
});

Console.WriteLine($"Contains 'CONFIGS/APPSETTINGS.JSON': {cache.Contains("CONFIGS/APPSETTINGS.JSON")}");
Console.WriteLine($"Get 'logs/app.log': {cache.Get("logs/app.log")}");
Console.WriteLine($"Remove 'logs/app.log': {cache.Remove("logs/app.log")}, Count={cache.Count}");

Console.WriteLine("\n=== Сценарій 2: TagManager (Dictionary + HashSet) ===");
var tagManager = new TagManager();
tagManager.AddTag("post-1", "C#");
tagManager.AddTag("post-1", ".NET");
tagManager.AddTag("post-1", "Collections");
tagManager.AddTag("post-1", "c#"); // дублікат ігнорується HashSet

tagManager.AddTag("post-2", "c#");
tagManager.AddTag("post-2", "ASP.NET");
tagManager.AddTag("post-2", "Collections");

Console.WriteLine($"Усі унікальні теги: {string.Join(", ", tagManager.GetAllUniqueTags())}");
Console.WriteLine($"Спільні теги post-1 та post-2: {string.Join(", ", tagManager.FindCommonTags("post-1", "post-2"))}");
Console.WriteLine($"Унікальні лише для post-1: {string.Join(", ", tagManager.FindExclusiveTags("post-1", "post-2"))}");
tagManager.RemoveTag("post-1", ".net");
Console.WriteLine($"Теги post-1 після видалення '.net': {string.Join(", ", tagManager.GetArticleTags("post-1"))}");

Console.WriteLine("\n=== Сценарій 3: SearchIndex (Dictionary + HashSet) ===");
var searchIndex = new SearchIndex();
searchIndex.AddDocument(1, "Dictionary and HashSet in C# provide fast lookup and set operations.");
searchIndex.AddDocument(2, "HashSet stores unique elements and supports IntersectWith and UnionWith.");
searchIndex.AddDocument(3, "Inverted index maps words to document IDs using Dictionary and HashSet.");

Console.WriteLine($"Документи зі словом 'hashset': [{string.Join(", ", searchIndex.Search("hashset"))}]");
Console.WriteLine($"Документи з 'dictionary' ТА 'hashset': [{string.Join(", ", searchIndex.SearchMultiple("dictionary", "hashset"))}]");
