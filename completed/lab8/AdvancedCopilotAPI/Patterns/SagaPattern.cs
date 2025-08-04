using System.Collections.Concurrent;

namespace AdvancedCopilotAPI.Patterns
{
    public abstract class SagaStep
    {
        public abstract Task<bool> ExecuteAsync(SagaContext context, CancellationToken cancellationToken = default);
        public abstract Task<bool> CompensateAsync(SagaContext context, CancellationToken cancellationToken = default);
        public abstract string GetStepName();
    }
    
    public class SagaContext
    {
        public Guid SagaId { get; } = Guid.NewGuid();
        public DateTime StartedAt { get; } = DateTime.UtcNow;
        public ConcurrentDictionary<string, object> Data { get; } = new();
        public List<string> ExecutedSteps { get; } = new();
        public List<string> CompensatedSteps { get; } = new();
        public string? FailureReason { get; set; }
        public Exception? LastException { get; set; }
        
        public T? GetData<T>(string key)
        {
            if (Data.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return default;
        }
        
        public void SetData<T>(string key, T value)
        {
            Data.AddOrUpdate(key, value!, (k, v) => value!);
        }
    }
    
    public class OrderProcessingSaga
    {
        private readonly ILogger<OrderProcessingSaga> _logger;
        private readonly List<SagaStep> _steps;
        
        public OrderProcessingSaga(ILogger<OrderProcessingSaga> logger)
        {
            _logger = logger;
            _steps = new List<SagaStep>
            {
                new ReserveInventoryStep(logger),
                new ChargePaymentStep(logger),
                new SendNotificationStep(logger)
            };
        }
        
        public async Task<bool> ExecuteAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting saga {SagaId}", context.SagaId);
            
            try
            {
                foreach (var step in _steps)
                {
                    _logger.LogInformation("Executing step {StepName} for saga {SagaId}",
                        step.GetStepName(), context.SagaId);
                    
                    var success = await step.ExecuteAsync(context, cancellationToken);
                    
                    if (success)
                    {
                        context.ExecutedSteps.Add(step.GetStepName());
                        _logger.LogInformation("Step {StepName} completed for saga {SagaId}",
                            step.GetStepName(), context.SagaId);
                    }
                    else
                    {
                        _logger.LogWarning("Step {StepName} failed for saga {SagaId}. Starting compensation.",
                            step.GetStepName(), context.SagaId);
                        
                        await CompensateAsync(context, cancellationToken);
                        return false;
                    }
                }
                
                _logger.LogInformation("Saga {SagaId} completed successfully", context.SagaId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saga {SagaId} failed with exception", context.SagaId);
                context.LastException = ex;
                context.FailureReason = ex.Message;
                
                await CompensateAsync(context, cancellationToken);
                return false;
            }
        }
        
        private async Task CompensateAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting compensation for saga {SagaId}", context.SagaId);
            
            var executedSteps = context.ExecutedSteps.ToList();
            executedSteps.Reverse();
            
            foreach (var stepName in executedSteps)
            {
                var step = _steps.FirstOrDefault(s => s.GetStepName() == stepName);
                if (step != null)
                {
                    try
                    {
                        _logger.LogInformation("Compensating step {StepName} for saga {SagaId}",
                            stepName, context.SagaId);
                        
                        await step.CompensateAsync(context, cancellationToken);
                        context.CompensatedSteps.Add(stepName);
                        
                        _logger.LogInformation("Step {StepName} compensated for saga {SagaId}",
                            stepName, context.SagaId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to compensate step {StepName} for saga {SagaId}",
                            stepName, context.SagaId);
                    }
                }
            }
            
            _logger.LogInformation("Compensation completed for saga {SagaId}", context.SagaId);
        }
    }
    
    public class ReserveInventoryStep : SagaStep
    {
        private readonly ILogger _logger;
        
        public ReserveInventoryStep(ILogger logger)
        {
            _logger = logger;
        }
        
        public override async Task<bool> ExecuteAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            var productId = context.GetData<int>("ProductId");
            var quantity = context.GetData<int>("Quantity");
            
            _logger.LogInformation("Reserving {Quantity} units of product {ProductId}", quantity, productId);
            
            await Task.Delay(100, cancellationToken);
            
            context.SetData("ReservationId", Guid.NewGuid());
            return true;
        }
        
        public override async Task<bool> CompensateAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            var reservationId = context.GetData<Guid>("ReservationId");
            
            _logger.LogInformation("Releasing reservation {ReservationId}", reservationId);
            
            await Task.Delay(50, cancellationToken);
            
            return true;
        }
        
        public override string GetStepName() => "ReserveInventory";
    }
    
    public class ChargePaymentStep : SagaStep
    {
        private readonly ILogger _logger;
        
        public ChargePaymentStep(ILogger logger)
        {
            _logger = logger;
        }
        
        public override async Task<bool> ExecuteAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            var amount = context.GetData<decimal>("Amount");
            var customerId = context.GetData<string>("CustomerId");
            
            _logger.LogInformation("Charging ${Amount} to customer {CustomerId}", amount, customerId);
            
            await Task.Delay(200, cancellationToken);
            
            context.SetData("TransactionId", Guid.NewGuid().ToString());
            return true;
        }
        
        public override async Task<bool> CompensateAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            var transactionId = context.GetData<string>("TransactionId");
            
            _logger.LogInformation("Refunding transaction {TransactionId}", transactionId);
            
            await Task.Delay(100, cancellationToken);
            
            return true;
        }
        
        public override string GetStepName() => "ChargePayment";
    }
    
    public class SendNotificationStep : SagaStep
    {
        private readonly ILogger _logger;
        
        public SendNotificationStep(ILogger logger)
        {
            _logger = logger;
        }
        
        public override async Task<bool> ExecuteAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            var customerId = context.GetData<string>("CustomerId");
            
            _logger.LogInformation("Sending confirmation notification to customer {CustomerId}", customerId);
            
            await Task.Delay(50, cancellationToken);
            
            context.SetData("NotificationId", Guid.NewGuid());
            return true;
        }
        
        public override async Task<bool> CompensateAsync(SagaContext context, CancellationToken cancellationToken = default)
        {
            var customerId = context.GetData<string>("CustomerId");
            
            _logger.LogInformation("Sending cancellation notification to customer {CustomerId}", customerId);
            
            await Task.Delay(50, cancellationToken);
            
            return true;
        }
        
        public override string GetStepName() => "SendNotification";
    }
}