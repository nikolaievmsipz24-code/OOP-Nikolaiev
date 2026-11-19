namespace lab13v19;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value) =>
        new(value, true, string.Empty);

    public static Result<T> Failure(string error) =>
        new(default, false, string.IsNullOrWhiteSpace(error) ? "Невідома помилка." : error);

    public static Result<T> Failure(Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        return new(default, false, ex.Message);
    }
}