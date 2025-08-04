using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// Fixed order processing system with proper error handling
var orderProcessor = new OrderProcessor();

try
{
    var orders = orderProcessor.GetOrders();
    Console.WriteLine("Processing Orders...\n");

    foreach (var order in orders)
    {
        try
        {
            var total = orderProcessor.CalculateTotal(order);
            Console.WriteLine($"Order {order.Id}: ${total:F2} - Customer: {order.CustomerEmail ?? "No email"}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing order {order.Id}: {ex.Message}");
        }
    }

    // Performance test (optional)
    Console.WriteLine("\nPerformance Test (Press Y to run, any other key to skip):");
    if (Console.ReadKey().Key == ConsoleKey.Y)
    {
        Console.WriteLine("\nRunning performance test...");
        var sw = System.Diagnostics.Stopwatch.StartNew();
        orderProcessor.ProcessLargeOrderSetOptimized();
        sw.Stop();
        Console.WriteLine($"Processing took: {sw.ElapsedMilliseconds}ms");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

class Order
{
    public int Id { get; set; }
    public List<OrderItem>? Items { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
}

class OrderItem
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal DiscountPercent { get; set; }
}

class OrderProcessor
{
    public List<Order> GetOrders()
    {
        return new List<Order>
        {
            new Order
            {
                Id = 1,
                CustomerEmail = "john@example.com",
                OrderDate = DateTime.Now.AddDays(-1),
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductName = "Laptop", Price = 999.99m, Quantity = 1, DiscountPercent = 0 },
                    new OrderItem { ProductName = "Mouse", Price = 29.99m, Quantity = 2, DiscountPercent = 10 }
                }
            },
            new Order
            {
                Id = 2,
                CustomerEmail = null,
                OrderDate = DateTime.Now.AddDays(-2),
                Items = null // This will demonstrate null handling
            },
            new Order
            {
                Id = 3,
                CustomerEmail = "jane@example.com",
                OrderDate = DateTime.Now.AddDays(-3),
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductName = "Keyboard", Price = 79.99m, Quantity = 0, DiscountPercent = 0 },
                    new OrderItem { ProductName = "Monitor", Price = 299.99m, Quantity = 1, DiscountPercent = 110 } // Invalid discount
                }
            },
            new Order
            {
                Id = 4,
                CustomerEmail = "bob@example.com",
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>() // Empty items list
            }
        };
    }

    public decimal CalculateTotal(Order order)
    {
        // Defensive programming: Check for null order
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order), "Order cannot be null");
        }

        // Handle null items collection
        if (order.Items == null || !order.Items.Any())
        {
            Console.WriteLine($"Warning: Order {order.Id} has no items");
            return 0;
        }

        decimal total = 0;
        foreach (var item in order.Items)
        {
            // Validate item
            if (item == null)
            {
                Console.WriteLine($"Warning: Null item found in order {order.Id}");
                continue;
            }

            // Calculate item total
            var itemTotal = item.Price * item.Quantity;
            
            // Validate and cap discount percentage
            var discountPercent = item.DiscountPercent;
            if (discountPercent < 0)
            {
                Console.WriteLine($"Warning: Negative discount ({discountPercent}%) for {item.ProductName}. Setting to 0%.");
                discountPercent = 0;
            }
            else if (discountPercent > 100)
            {
                Console.WriteLine($"Warning: Discount exceeds 100% ({discountPercent}%) for {item.ProductName}. Capping at 100%.");
                discountPercent = 100;
            }

            var discount = itemTotal * (discountPercent / 100);
            var finalPrice = itemTotal - discount;
            
            // Log calculation details for debugging
            if (item.Quantity > 0)
            {
                Console.WriteLine($"  {item.ProductName}: ${item.Price} x {item.Quantity} - {discountPercent}% = ${finalPrice:F2}");
            }
            
            total += finalPrice;
        }

        return total;
    }

    public void ProcessLargeOrderSet()
    {
        var orders = new List<Order>();
        // Simulate large dataset
        for (int i = 0; i < 10000; i++)
        {
            orders.Add(new Order
            {
                Id = i,
                Items = GetOrders()[0].Items
            });
        }
        
        // Inefficient processing (intentionally slow)
        foreach (var order in orders)
        {
            var matchingOrders = orders.Where(o => o.Id == order.Id).ToList();
            var total = CalculateTotal(order);
            Thread.Sleep(1); // Simulate slow operation
        }
    }

    public void ProcessLargeOrderSetOptimized()
    {
        var orders = new List<Order>();
        var baseItems = GetOrders()[0].Items;
        
        // Simulate large dataset
        for (int i = 0; i < 10000; i++)
        {
            orders.Add(new Order
            {
                Id = i,
                Items = baseItems
            });
        }
        
        // Optimized processing
        var orderDict = orders.ToDictionary(o => o.Id); // O(1) lookup instead of O(n)
        
        // Process in parallel for better performance
        Parallel.ForEach(orders, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, order =>
        {
            // Direct dictionary lookup instead of LINQ Where
            if (orderDict.TryGetValue(order.Id, out var matchingOrder))
            {
                var total = CalculateTotal(order);
            }
            // Removed Thread.Sleep for better performance
        });
    }
}