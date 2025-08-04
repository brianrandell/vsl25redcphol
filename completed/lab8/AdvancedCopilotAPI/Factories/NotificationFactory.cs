using AdvancedCopilotAPI.Services;

namespace AdvancedCopilotAPI.Factories
{
    public class EmailNotificationService : INotificationService
    {
        private readonly ILogger<EmailNotificationService> _logger;
        
        public EmailNotificationService(ILogger<EmailNotificationService> logger)
        {
            _logger = logger;
        }
        
        public async Task<bool> SendNotificationAsync(string recipient, string subject, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending email to {Recipient} with subject: {Subject}", recipient, subject);
            
            await Task.Delay(100, cancellationToken);
            
            _logger.LogInformation("Email sent successfully to {Recipient}", recipient);
            return true;
        }
        
        public string GetNotificationType() => "Email";
    }
    
    public class SmsNotificationService : INotificationService
    {
        private readonly ILogger<SmsNotificationService> _logger;
        
        public SmsNotificationService(ILogger<SmsNotificationService> logger)
        {
            _logger = logger;
        }
        
        public async Task<bool> SendNotificationAsync(string recipient, string subject, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending SMS to {Recipient}: {Message}", recipient, message);
            
            await Task.Delay(50, cancellationToken);
            
            _logger.LogInformation("SMS sent successfully to {Recipient}", recipient);
            return true;
        }
        
        public string GetNotificationType() => "SMS";
    }
    
    public class PushNotificationService : INotificationService
    {
        private readonly ILogger<PushNotificationService> _logger;
        
        public PushNotificationService(ILogger<PushNotificationService> logger)
        {
            _logger = logger;
        }
        
        public async Task<bool> SendNotificationAsync(string recipient, string subject, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending push notification to {Recipient} with title: {Subject}", recipient, subject);
            
            await Task.Delay(30, cancellationToken);
            
            _logger.LogInformation("Push notification sent successfully to {Recipient}", recipient);
            return true;
        }
        
        public string GetNotificationType() => "Push";
    }
    
    public class InAppNotificationService : INotificationService
    {
        private readonly ILogger<InAppNotificationService> _logger;
        
        public InAppNotificationService(ILogger<InAppNotificationService> logger)
        {
            _logger = logger;
        }
        
        public async Task<bool> SendNotificationAsync(string recipient, string subject, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating in-app notification for {Recipient}: {Subject}", recipient, subject);
            
            await Task.Delay(10, cancellationToken);
            
            _logger.LogInformation("In-app notification created successfully for {Recipient}", recipient);
            return true;
        }
        
        public string GetNotificationType() => "InApp";
    }
    
    public class NotificationFactory : INotificationFactory
    {
        private readonly IServiceProvider _serviceProvider;
        
        public NotificationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public INotificationService CreateNotificationService(NotificationType type)
        {
            return type switch
            {
                NotificationType.Email => _serviceProvider.GetRequiredService<EmailNotificationService>(),
                NotificationType.Sms => _serviceProvider.GetRequiredService<SmsNotificationService>(),
                NotificationType.Push => _serviceProvider.GetRequiredService<PushNotificationService>(),
                NotificationType.InApp => _serviceProvider.GetRequiredService<InAppNotificationService>(),
                _ => throw new ArgumentException($"Unsupported notification type: {type}")
            };
        }
        
        public IEnumerable<INotificationService> CreateAllNotificationServices()
        {
            return new[]
            {
                CreateNotificationService(NotificationType.Email),
                CreateNotificationService(NotificationType.Sms),
                CreateNotificationService(NotificationType.Push),
                CreateNotificationService(NotificationType.InApp)
            };
        }
    }
}