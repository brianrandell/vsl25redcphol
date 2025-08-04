namespace AdvancedCopilotAPI.Strategies
{
    public enum CustomerType
    {
        Regular,
        Member,
        Premium
    }
    
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    
    public class PricingContext
    {
        public int Quantity { get; set; }
        public CustomerType CustomerType { get; set; }
        public Season Season { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsLoyaltyMember { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
    
    public interface IPricingStrategy
    {
        decimal CalculatePrice(PricingContext context);
        string GetStrategyName();
        string GetDescription();
    }
    
    public class RegularPricingStrategy : IPricingStrategy
    {
        public decimal CalculatePrice(PricingContext context)
        {
            return context.BasePrice * context.Quantity;
        }
        
        public string GetStrategyName() => "Regular Pricing";
        
        public string GetDescription() => "Standard pricing with no discounts";
    }
    
    public class BulkDiscountPricingStrategy : IPricingStrategy
    {
        private const decimal BulkDiscountPercentage = 0.10m;
        private const int BulkThreshold = 10;
        
        public decimal CalculatePrice(PricingContext context)
        {
            var totalPrice = context.BasePrice * context.Quantity;
            
            if (context.Quantity >= BulkThreshold)
            {
                totalPrice *= (1 - BulkDiscountPercentage);
            }
            
            return totalPrice;
        }
        
        public string GetStrategyName() => "Bulk Discount Pricing";
        
        public string GetDescription() => $"{BulkDiscountPercentage:P0} off for {BulkThreshold}+ items";
    }
    
    public class MemberPricingStrategy : IPricingStrategy
    {
        private const decimal MemberDiscountPercentage = 0.15m;
        private const decimal PremiumDiscountPercentage = 0.20m;
        
        public decimal CalculatePrice(PricingContext context)
        {
            var totalPrice = context.BasePrice * context.Quantity;
            
            var discountPercentage = context.CustomerType switch
            {
                CustomerType.Member => MemberDiscountPercentage,
                CustomerType.Premium => PremiumDiscountPercentage,
                _ => 0m
            };
            
            if (context.IsLoyaltyMember)
            {
                discountPercentage += 0.05m;
            }
            
            return totalPrice * (1 - discountPercentage);
        }
        
        public string GetStrategyName() => "Member Pricing";
        
        public string GetDescription() => $"Member: {MemberDiscountPercentage:P0} off, Premium: {PremiumDiscountPercentage:P0} off, +5% for loyalty";
    }
    
    public class SeasonalPricingStrategy : IPricingStrategy
    {
        private readonly Dictionary<Season, decimal> _seasonalDiscounts = new()
        {
            { Season.Spring, 0.05m },
            { Season.Summer, 0.10m },
            { Season.Fall, 0.15m },
            { Season.Winter, 0.20m }
        };
        
        public decimal CalculatePrice(PricingContext context)
        {
            var totalPrice = context.BasePrice * context.Quantity;
            
            if (_seasonalDiscounts.TryGetValue(context.Season, out var discount))
            {
                totalPrice *= (1 - discount);
            }
            
            return totalPrice;
        }
        
        public string GetStrategyName() => "Seasonal Pricing";
        
        public string GetDescription() => "Seasonal discounts: Spring 5%, Summer 10%, Fall 15%, Winter 20%";
    }
    
    public class CombinedPricingStrategy : IPricingStrategy
    {
        private readonly List<IPricingStrategy> _strategies;
        
        public CombinedPricingStrategy(params IPricingStrategy[] strategies)
        {
            _strategies = strategies.ToList();
        }
        
        public decimal CalculatePrice(PricingContext context)
        {
            var prices = _strategies.Select(strategy => strategy.CalculatePrice(context)).ToList();
            
            return prices.Min();
        }
        
        public string GetStrategyName() => "Combined Pricing";
        
        public string GetDescription() => $"Best price from {_strategies.Count} strategies: {string.Join(", ", _strategies.Select(s => s.GetStrategyName()))}";
    }
    
    public class PricingCalculator
    {
        private readonly Dictionary<string, IPricingStrategy> _strategies;
        
        public PricingCalculator()
        {
            _strategies = new Dictionary<string, IPricingStrategy>
            {
                ["regular"] = new RegularPricingStrategy(),
                ["bulk"] = new BulkDiscountPricingStrategy(),
                ["member"] = new MemberPricingStrategy(),
                ["seasonal"] = new SeasonalPricingStrategy(),
                ["combined"] = new CombinedPricingStrategy(
                    new BulkDiscountPricingStrategy(),
                    new MemberPricingStrategy(),
                    new SeasonalPricingStrategy())
            };
        }
        
        public decimal CalculatePrice(string strategyName, PricingContext context)
        {
            if (!_strategies.TryGetValue(strategyName.ToLowerInvariant(), out var strategy))
            {
                throw new ArgumentException($"Unknown pricing strategy: {strategyName}");
            }
            
            return strategy.CalculatePrice(context);
        }
        
        public IEnumerable<string> GetAvailableStrategies()
        {
            return _strategies.Keys;
        }
        
        public string GetStrategyDescription(string strategyName)
        {
            if (_strategies.TryGetValue(strategyName.ToLowerInvariant(), out var strategy))
            {
                return strategy.GetDescription();
            }
            
            return "Unknown strategy";
        }
    }
}