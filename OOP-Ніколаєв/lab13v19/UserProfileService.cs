namespace lab13v19;

public class UserProfileService
{
    private readonly Dictionary<int, UserProfile> _users = new()
    {
        [1] = new UserProfile { Id = 1, Username = "maksym_dev", Email = "maksym@example.com", Age = 17 },
        [2] = new UserProfile { Id = 2, Username = "emilia_ui", Email = "emilia@example.com", Age = 18 }
    };

    public void UpdateProfile(UserProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        if (string.IsNullOrWhiteSpace(profile.Username) || profile.Username.Length < 3)
        {
            throw new InvalidUserDataException(
                nameof(profile.Username),
                profile.Username,
                "Ім'я користувача повинно містити щонайменше 3 символи.");
        }

        if (profile.Age < 13 || profile.Age > 120)
        {
            throw new InvalidUserDataException(
                nameof(profile.Age),
                profile.Age.ToString(),
                "Вік користувача має бути в межах від 13 до 120 років.");
        }

        if (string.IsNullOrWhiteSpace(profile.Email) || !profile.Email.Contains('@'))
        {
            throw new InvalidUserDataException(
                nameof(profile.Email),
                profile.Email,
                "Некоректний формат електронної пошти.");
        }

        _users[profile.Id] = new UserProfile
        {
            Id = profile.Id,
            Username = profile.Username,
            Email = profile.Email,
            Age = profile.Age
        };
    }

    public Result<bool> ChangeEmail(int userId, string newEmail)
    {
        if (!_users.TryGetValue(userId, out UserProfile? user))
        {
            return Result<bool>.Failure($"Користувача з ID={userId} не знайдено.");
        }

        if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains('@') || !newEmail.Contains('.'))
        {
            return Result<bool>.Failure($"Email '{newEmail}' має некоректний формат.");
        }

        bool isTakenByOtherUser = _users.Values.Any(u =>
            u.Id != userId && string.Equals(u.Email, newEmail, StringComparison.OrdinalIgnoreCase));

        if (isTakenByOtherUser)
        {
            return Result<bool>.Failure($"Email '{newEmail}' уже використовується іншим користувачем.");
        }

        user.Email = newEmail;
        return Result<bool>.Success(true);
    }
}