namespace ProductInventoryAPI.Exceptions;

public class BusinessRuleException : ApiException
{
    public BusinessRuleException(string message) 
        : base(422, "Business Rule Violation", message)
    {
    }

    public BusinessRuleException(string message, Exception innerException) 
        : base(422, "Business Rule Violation", message, innerException)
    {
    }
}