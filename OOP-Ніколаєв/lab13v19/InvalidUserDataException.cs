namespace lab13v19;

public class InvalidUserDataException : Exception
{
    public string Field { get; } = string.Empty;
    public string Value { get; } = string.Empty;

    public InvalidUserDataException()
    {
    }

    public InvalidUserDataException(string message)
        : base(message)
    {
    }

    public InvalidUserDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public InvalidUserDataException(string field, string value, string? message = null)
        : base(message ?? $"Некоректне значення '{value}' для поля '{field}'.")
    {
        Field = field;
        Value = value;
    }

    public InvalidUserDataException(string field, string value, string message, Exception innerException)
        : base(message, innerException)
    {
        Field = field;
        Value = value;
    }
}