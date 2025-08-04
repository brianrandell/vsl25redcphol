namespace ProductInventoryAPI.Exceptions;

public class NotFoundException : ApiException
{
    public NotFoundException(string message) 
        : base(404, "Not Found", message)
    {
    }

    public NotFoundException(string resourceType, object resourceId) 
        : base(404, "Not Found", $"{resourceType} with ID {resourceId} was not found.")
    {
    }
}