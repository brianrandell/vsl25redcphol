namespace ProductInventoryAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        // Since we don't have authentication in this demo, return a default user ID
        // In a real application, this would get the user ID from claims
        var userIdFromHeader = _httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"].FirstOrDefault();
        return userIdFromHeader ?? "system";
    }

    public string GetCurrentUserName()
    {
        // Since we don't have authentication in this demo, return a default username
        // In a real application, this would get the username from claims
        var userNameFromHeader = _httpContextAccessor.HttpContext?.Request.Headers["X-User-Name"].FirstOrDefault();
        return userNameFromHeader ?? "System User";
    }
}