namespace Shared.Models;

public class Result
{
    public bool IsSuccessful { get; }
    public string ErrorMessage { get; }

    private Result(bool isSuccessful, string errorMessage)
    {
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
    }

    public static Result Successful() => new(true, string.Empty);

    public static Result Error(string errorMessage) => new(false, errorMessage);
}
