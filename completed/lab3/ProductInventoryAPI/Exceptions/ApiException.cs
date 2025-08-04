namespace ProductInventoryAPI.Exceptions;

public abstract class ApiException : Exception
{
    public int StatusCode { get; }
    public string Title { get; }

    protected ApiException(int statusCode, string title, string message) : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }

    protected ApiException(int statusCode, string title, string message, Exception innerException) 
        : base(message, innerException)
    {
        StatusCode = statusCode;
        Title = title;
    }
}