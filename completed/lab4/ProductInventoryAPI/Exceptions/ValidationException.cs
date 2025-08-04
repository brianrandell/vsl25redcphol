namespace ProductInventoryAPI.Exceptions;

public class ValidationException : ApiException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message) 
        : base(400, "Validation Error", message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors) 
        : base(400, "Validation Error", "One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string field, string message) 
        : base(400, "Validation Error", message)
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { message } }
        };
    }
}