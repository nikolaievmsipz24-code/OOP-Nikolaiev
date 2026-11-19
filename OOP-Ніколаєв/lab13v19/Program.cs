using lab13v19;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var service = new UserProfileService();

Console.WriteLine("=== 1. UpdateProfile: валідні та невалідні дані (InvalidUserDataException) ===");

var validProfile = new UserProfile
{
    Id = 1,
    Username = "maksym_rcit",
    Email = "maksym@example.com",
    Age = 17
};

try
{
    service.UpdateProfile(validProfile);
    Console.WriteLine($"Профіль користувача #{validProfile.Id} успішно оновлено ({validProfile.Username}).");
}
catch (InvalidUserDataException ex)
{
    Console.WriteLine($"Помилка валідації [Поле: {ex.Field}, Значення: '{ex.Value}']: {ex.Message}");
}

var invalidProfile = new UserProfile
{
    Id = 1,
    Username = "m",
    Email = "maksym@example.com",
    Age = 17
};

try
{
    service.UpdateProfile(invalidProfile);
    Console.WriteLine("Профіль оновлено.");
}
catch (InvalidUserDataException ex)
{
    Console.WriteLine($"Перехоплено виняток! Поле: '{ex.Field}', Значення: '{ex.Value}', Причина: {ex.Message}");
}

Console.WriteLine("\n=== 2. ChangeEmail: обробка через патерн Result<bool> ===");

Result<bool> takenEmailResult = service.ChangeEmail(1, "emilia@example.com");
if (takenEmailResult.IsSuccess)
{
    Console.WriteLine("Email успішно змінено.");
}
else
{
    Console.WriteLine($"Зміну email відхилено (Result.Failure): {takenEmailResult.Error}");
}

Result<bool> freeEmailResult = service.ChangeEmail(1, "maksym.new@example.com");
if (freeEmailResult.IsSuccess)
{
    Console.WriteLine($"Email успішно змінено (Result.Success = {freeEmailResult.Value}).");
}
else
{
    Console.WriteLine($"Зміну email відхилено: {freeEmailResult.Error}");
}