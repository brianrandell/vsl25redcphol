namespace AdvancedCopilotAPI.Factories
{
    public enum NotificationType
    {
        Email,
        Sms,
        Push,
        InApp
    }
    
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(string recipient, string subject, string message, CancellationToken cancellationToken = default);
        string GetNotificationType();
    }
    
    public interface INotificationFactory
    {
        INotificationService CreateNotificationService(NotificationType type);
        IEnumerable<INotificationService> CreateAllNotificationServices();
    }
}