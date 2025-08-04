# Lab 5: Debugging with Copilot - Hands-on Exercise

**Duration:** 20 minutes

## Prerequisites

* Visual Studio 2022 with GitHub Copilot enabled
* Completed previous labs or equivalent experience
* Basic understanding of debugging in Visual Studio

## Exercise Overview

Learn how to use GitHub Copilot to assist with debugging, analyze exceptions, and optimize performance issues in your applications.

---

## Part 1: Debug Integration Setup (5 minutes)

### Step 1: Create a Buggy Application

1. **Create a new Console App:**

   * File → New → Project
   * Select "Console App" (C#)
   * Name: `DebuggingWithCopilot`
   * Framework: .NET 9.0

2. **Replace Program.cs with this buggy code:**

   ```csharp
   // Buggy order processing system
   var orderProcessor = new OrderProcessor();
   var orders = orderProcessor.GetOrders();

   foreach (var order in orders)
   {
       var total = orderProcessor.CalculateTotal(order);
       Console.WriteLine($"Order {order.Id}: ${total}");
   }

   class Order
   {
       public int Id { get; set; }
       public List<OrderItem> Items { get; set; }
       public string CustomerEmail { get; set; }
       public DateTime OrderDate { get; set; }
   }

   class OrderItem
   {
       public string ProductName { get; set; }
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
                   Items = new List<OrderItem>
                   {
                       new OrderItem { ProductName = "Laptop", Price = 999.99m, Quantity = 1 },
                       new OrderItem { ProductName = "Mouse", Price = 29.99m, Quantity = 2 }
                   }
               },
               new Order
               {
                   Id = 2,
                   CustomerEmail = null,
                   Items = null
               },
               new Order
               {
                   Id = 3,
                   CustomerEmail = "jane@example.com",
                   Items = new List<OrderItem>
                   {
                       new OrderItem { ProductName = "Keyboard", Price = 79.99m, Quantity = 0 },
                       new OrderItem { ProductName = "Monitor", Price = 299.99m, Quantity = 1, DiscountPercent = 110 }
                   }
               }
           };
       }

       public decimal CalculateTotal(Order order)
       {
           decimal total = 0;
           foreach (var item in order.Items)
           {
               var itemTotal = item.Price * item.Quantity;
               var discount = itemTotal * (item.DiscountPercent / 100);
               total += itemTotal - discount;
           }
           return total;
       }
   }
   ```

3. **Run the application** (F5) - It will crash!

---

## Part 2: Debug with Copilot Assistance (7 minutes)

### Step 1: Analyze the Exception

1. **When the exception occurs:**

   * Visual Studio breaks on the exception
   * Note the NullReferenceException

2. **Click the Analyze with Copilot link**

   Wait for Copilot to analyze the exception and read the results.

3. Stop the debugger (Shift + F5).

4. **Select the CalculateTotal method** and ask:

   ```
   Add null checks and defensive programming to prevent NullReferenceException
   ```

5. **Apply the suggested fix**

### Step 2: Set Breakpoints and Analyze

1. **Set a breakpoint** at the beginning of CalculateTotal method (F9)

2. **Run the debugger** (F5)

3. **When breakpoint hits:**

   * Hover over variables
   * Check the Locals window

4. **In Copilot Chat, ask:**

   ```
   The order.Items is null for order ID 2. What's the best practice for handling null collections in C#?
   ```

5. **While still at breakpoint, ask:**

   ```
   How can I add logging to track which orders are causing issues?
   ```

### Step 3: Fix Logic Errors

1. **Continue debugging** (F5) until the next issue

2. **Notice the discount calculation issue** (110% discount)

3. **Select the discount calculation line** and ask Copilot:

   ```
   This discount calculation allows discounts over 100%. How should I validate and cap discount percentages?
   ```

4. **Apply the validation logic**

---

## Part 3: Exception Analysis (5 minutes)

### Step 1: Add Comprehensive Error Handling

1. **Select the entire Program.cs content**

2. **In Copilot Chat with Agent Mode, request:**

   ```
   Add comprehensive exception handling with:
   - Try-catch blocks around risky operations
   - Specific exception types
   - Detailed error logging
   - Continue processing other orders if one fails
   ```

3. **Review the changes** made by Agent Mode

### Step 2: Analyze Stack Traces

1. **Intentionally create a new error:**

   ```csharp
   // Add this line in GetOrders method
   throw new InvalidOperationException("Database connection failed", 
       new SqlException("Connection timeout"));
   ```

2. **Run the application** and when it crashes

3. **Copy the stack trace** and ask Copilot:

   ```
   Analyze this stack trace and explain what went wrong:
   [paste stack trace]
   ```

4. **Follow up with:**

   ```
   How would I implement retry logic for transient database errors?
   ```

---

## Part 4: Performance Debugging (5 minutes)

### Step 1: Identify Performance Issues

1. **Add this slow method to OrderProcessor:**

   ```csharp
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
       
       // Inefficient processing
       foreach (var order in orders)
       {
           var matchingOrders = orders.Where(o => o.Id == order.Id).ToList();
           var total = CalculateTotal(order);
           Thread.Sleep(1); // Simulate slow operation
       }
   }
   ```

2. **Add to Main method:**

   ```csharp
   var sw = System.Diagnostics.Stopwatch.StartNew();
   orderProcessor.ProcessLargeOrderSet();
   sw.Stop();
   Console.WriteLine($"Processing took: {sw.ElapsedMilliseconds}ms");
   ```

### Step 2: Optimize with Copilot

1. **Select the ProcessLargeOrderSet method**

2. **Ask Copilot:**

   ```
   This method is very slow. Identify performance issues and suggest optimizations.
   ```

3. **Apply suggested improvements**

4. **Ask for profiling guidance:**

   ```
   How can I use Visual Studio's Performance Profiler to identify bottlenecks in this code?
   ```

### Step 3: Memory Debugging

1. **Ask Copilot:**

   ```
   How can I check if this method has memory leaks or excessive allocations?
   ```

2. **Follow up with:**

   ```
   Show me how to use diagnostic tools in Visual Studio to monitor memory usage while debugging
   ```

---

## Key Takeaways

✅ **You've learned to:**

* Use Copilot to analyze exceptions and stack traces
* Get debugging suggestions while at breakpoints
* Add defensive programming based on runtime errors
* Optimize performance issues with Copilot's help
* Implement comprehensive error handling

## Best Practices for Debugging with Copilot

1. **Provide Context:**

   * Include error messages
   * Share variable states
   * Describe the expected behavior

2. **Use Copilot During Debugging:**

   * Ask about variable values
   * Get suggestions for fixes
   * Understand error root causes

3. **Iterative Debugging:**

   * Fix one issue at a time
   * Re-run to find next issue
   * Ask Copilot for patterns

## Challenge Extension (Optional)

1. **Add these debugging scenarios:**

   * Deadlock detection
   * Memory leak investigation
   * Async/await debugging
   * Collection modification errors

2. **Ask Copilot:**

   ```
   Create a unit test that reproduces the NullReferenceException bug we fixed
   ```

## Troubleshooting Tips

**Copilot not understanding the error?**

* Provide the full exception message
* Include relevant code context
* Mention the line number

**Debugging suggestions too complex?**

* Ask for "simple" or "basic" solutions first
* Request step-by-step guidance
* Ask for explanations of suggestions
