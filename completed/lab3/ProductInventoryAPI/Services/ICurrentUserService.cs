namespace ProductInventoryAPI.Services;

public interface ICurrentUserService
{
    string GetCurrentUserId();
    string GetCurrentUserName();
}