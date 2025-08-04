# Lab 3: Copilot Chat & Contextual Assistance - Hands-on Exercise

**Duration:** 50 minutes

## Prerequisites

- Visual Studio 2022 with GitHub Copilot Chat enabled
- Completed Lab 1 & 2 or equivalent experience
- Basic understanding of ASP.NET Core Web API

## Exercise Overview

Create a simple Web API using Copilot Chat to explain code, generate controllers, add error handling, and create DTOs with mapping logic.

---

## Part 1: Create Web API Project (3 minutes)

1. **Create a new project:**

   - File → New → Project
   - Search for "ASP.NET Core Web API"
   - Select the template and click `Next`
   - Name: `ProductInventoryAPI`
   - Click `Next`

2. **Configure the project:**

   - Framework: .NET 9.0
   - Authentication: None
   - Configure for HTTPS: ✓
   - Enable OpenAPI support: ✓
   - Use controllers: ✓
   - Click `Create`

3. **Open Copilot Chat:**

   - View → GitHub Copilot Chat
   - Dock the window to your preferred location

---

## Part 2: Understanding the Boilerplate with Chat (5 minutes)

### Step 1: Explore Program.cs

1. **Open `Program.cs`**

2. **Select ALL the code** in Program.cs (Ctrl+A)

3. **In Copilot Chat, type:**

   ```
   Explain this code line by line, especially the service configuration and middleware pipeline
   ```

4. **Read through the explanation** and note:

   - Service registration
   - Middleware order
   - Swagger configuration

### Step 2: Understand the Weather Controller

1. **Open `Controllers/WeatherForecastController.cs`**


2. **Ask Copilot Chat:**

   ```
   What does this controller do and what are the key attributes used?
   ```

   > Notice how you didn't have to select any code, the Active Document context is used by default.

3. **Follow up with:**

   ```
   How would I modify this to return data from a database instead?
   ```

---

## Part 3: Generate a Product API with Chat Assistance (10 minutes)

### Step 1: Create the Product Model

1. **Create a new folder** called `Models`

2. **In Copilot Chat, type:**

   ```
   Create a Product model class with properties: Id (int), Name (string), Description (string), Price (decimal), StockQuantity (int), Category (string), and IsActive (bool). Include data annotations for validation.
   ```

3. **Copy the generated code** and create `Models/Product.cs`

4. **Ask Copilot Chat:**

   ```
   What NuGet package do I need for the data annotations?
   ```

   > You shouldn't need to install any additional packages as the data annotations are part of the `System.ComponentModel.DataAnnotations` namespace, which is included in .NET Core by default.

### Step 2: Generate the Controller

1. **Right-click Controllers folder** → Add → Controller → API Controller - Empty → Name: `ProductsController.cs`

2. **Make sure the empty controller is the active tab.**

3. **In Copilot Chat, type:**

   ```
   Transform this into a full CRUD controller for Product model with:
   - In-memory list for data storage
   - All HTTP verbs (GET all, GET by id, POST, PUT, DELETE)
   - Proper HTTP status codes
   - Basic validation
   ```

4. **Replace your controller code** with Copilot's suggestion by clicking the Apply button in the chat window and then Tab to accept.

### Step 3: Enhance with Contextual Help

1. **Select just the GET method** in your controller

2. **Ask Copilot Chat:**

   ```
   Add pagination to this method with pageNumber and pageSize parameters
   ```

3. **Select the POST method**

4. **Ask:**

   ```
   Add validation to ensure the product name is unique before adding
   ```

---

## Part 4: Add Error Handling with Chat Guidance (8 minutes)

### Step 1: Global Error Handling

1. **In Copilot Chat, ask:**

   ```
   How do I add global error handling middleware to my ASP.NET Core Web API?
   ```

2. **Follow up with:**

   ```
   Create a custom exception handling middleware class that returns consistent error responses in JSON format
   ```

3. **Create `Middleware/ErrorHandlingMiddleware.cs`** with the generated code

4. **Ask Copilot Chat:**

   ```
   Show me how to register this middleware in Program.cs
   ```

### Step 2: Custom Exceptions

1. **Ask Copilot Chat:**

   ```
   Create custom exception classes for:
   - NotFoundException
   - ValidationException  
   - BusinessRuleException
   All should inherit from a base ApiException class
   ```

2. **Create `Exceptions` folder** and add the generated classes

3. **Select your ProductsController**

4. **Ask Copilot Chat:**

   ```
   Refactor this controller to use the custom exceptions instead of returning status codes directly
   ```

---

## Part 5: Create DTOs and Mapping (8 minutes)

### Step 1: Generate DTOs

1. **In Copilot Chat, type:**

   ```
   Create DTOs for the Product model:
   - ProductCreateDto (for POST)
   - ProductUpdateDto (for PUT)  
   - ProductResponseDto (for GET responses)
   Explain why we use DTOs instead of domain models
   ```

2. **Create `DTOs` folder** and add the generated classes

### Step 2: Add Mapping Logic

1. **Ask Copilot Chat:**

   ```
   Create an extension method class to map between Product entity and the DTOs
   ```

2. **Create `Extensions/ProductMappingExtensions.cs`**

3. **Select your ProductsController again**

4. **Ask Copilot Chat:**

   ```
   Refactor this controller to use the DTOs instead of the Product model directly
   ```

### Step 3: Understand the Changes

1. **Select the refactored POST method**

2. **Ask Copilot Chat:**

   ```
   Explain the flow of data in this method from DTO to entity and back
   ```

---

## Part 6: Add API Documentation (6 minutes)

### Step 1: Enhance with XML Comments

1. **Select your entire ProductsController**

2. **In Copilot Chat, ask:**

   ```
   Add XML documentation comments to all methods including:
   - Summary of what the method does
   - Parameter descriptions
   - Response types and status codes
   - Example requests where applicable
   ```
   Put the updated code from the chat into your ProductsController however you see fit.

3. **Ask follow-up:**

   ```
   How do I configure Swagger to show these XML comments?
   ```

4. **Apply the suggested Program.cs changes**

### Step 2: Add API Versioning

1. **Ask Copilot Chat:**

   ```
   How do I add API versioning to my Web API? Show me the simplest approach.
   ```

2. **Follow up with:**

   ```
   Update my ProductsController to support API version 1.0
   ```

---

## Testing Your API (3 minutes)

1. **Run the application** (F5)

2. **Open Swagger UI** (browser should open automatically)

3. **Test each endpoint:**

   - Create products
   - Get all products (test pagination)
   - Update a product
   - Test error scenarios

4. **In Copilot Chat, ask:**

   ```
   Generate a set of curl commands to test all my Product API endpoints
   ```

---

## Key Takeaways

✅ **You've learned to:**

- Use Copilot Chat to explain existing code
- Generate entire controllers with specific requirements
- Refactor code using contextual chat
- Add cross-cutting concerns like error handling
- Create DTOs and mapping logic with explanations
- Enhance APIs with documentation

## Advanced Chat Techniques

### Context References

- **#file** - Reference specific files in chat
- **#selection** - Talk about selected code
- **Multiple selections** - Hold Ctrl to select multiple code blocks

### Effective Prompts

1. **Be specific**: "Add pagination with 10 items per page default"
2. **Ask for explanations**: "Explain why..." or "What are the benefits..."
3. **Request alternatives**: "Show me 3 different ways to..."
4. **Iterative refinement**: Build on previous responses

## Challenge Extensions (Optional)

1. **Ask Copilot Chat to:**

   - Add caching to the GET endpoints
   - Implement soft delete functionality
   - Add rate limiting
   - Create integration tests

2. **Advanced scenarios:**

   ```
   How would I add authentication to this API using JWT tokens?
   ```

   ```
   Show me how to add a database context using Entity Framework Core
   ```

---

## Part 7: Ask Mode vs Agent Mode (15 minutes)

### Understanding the Difference

1. **Open Copilot Chat** if not already open

2. **Notice the mode selector:**

   - Look for the dropdown that shows "Ask" by default
   - This is where you can switch between modes

3. **Key differences:**

   - **Ask Mode**: Interactive Q&A, you control all actions
   - **Agent Mode**: Autonomous task completion, Copilot can make changes directly

### When to Use Each Mode

**Use Ask Mode for:**

- Learning and understanding code
- Getting explanations
- Exploring options before deciding
- When you want full control

**Use Agent Mode for:**

- Implementing complete features
- Refactoring across multiple files
- Repetitive tasks
- When you trust Copilot to make changes

### Hands-on Exercise: Ask Mode

1. **Ensure you're in Ask Mode** (default)

2. **Select your ProductsController**

3. **In Copilot Chat, type:**

   ```
   How can I add filtering capabilities to the GET endpoint? Show me different approaches.
   ```

4. **Notice how Copilot:**

   - Provides options
   - Explains trade-offs
   - Shows code examples
   - But doesn't modify your files

5. **Follow up with:**

   ```
   Which approach would be best for a production API?
   ```

6. **Manually implement** the suggested approach

### Hands-on Exercise: Agent Mode

1. **Switch to Agent Mode:**

   - Click the dropdown showing "Ask"
   - Select "Agent"
   - Notice the confirmation message

2. **Make a high-level request:**

   ```
   Add comprehensive logging to all methods in ProductsController using ILogger. Include:
   - Method entry/exit logs
   - Parameter values
   - Execution time
   - Error details in catch blocks
   ```

3. **Watch Agent Mode work:**

   - Copilot analyzes your controller
   - Makes changes directly to the file
   - Adds necessary using statements
   - Updates constructor for dependency injection

4. **Review the changes:**

   - Check the modifications
   - Notice the consistency
   - Undo if needed (Ctrl+Z)

### Comparing the Modes

1. **Switch back to Ask Mode**

2. **Try this request:**

   ```
   Add input sanitization to prevent XSS attacks in the Product name and description
   ```

3. **Notice in Ask Mode:**

   - You get explanations and code samples
   - You decide what to implement
   - You maintain full control

4. **Switch to Agent Mode**

5. **Try the same request:**

   ```
   Add input sanitization to prevent XSS attacks in the Product name and description
   ```

6. **Notice in Agent Mode:**

   - Copilot implements directly
   - Makes consistent changes
   - Updates all relevant methods

### Best Practices for Each Mode

**Ask Mode Best Practices:**

1. Use for exploration and learning
2. Great for architectural decisions
3. Perfect when you need explanations
4. Ideal for understanding existing code

**Agent Mode Best Practices:**

1. Be specific about requirements
2. Review changes before committing
3. Use for well-defined tasks
4. Great for refactoring and cleanup

### Advanced Agent Mode Task

1. **In Agent Mode, request:**

   ```
   Create a complete audit trail system for the Product entity:
   - Add CreatedAt, UpdatedAt, CreatedBy, UpdatedBy properties
   - Create an IAuditableEntity interface
   - Update all CRUD operations to set these values
   - Add a middleware to capture the current user
   ```

2. **Observe how Agent Mode:**

   - Creates the interface
   - Updates the model
   - Modifies the controller
   - Might even create middleware

### Mode Selection Guidelines

Choose **Ask Mode** when you:

- Need to understand "why" or "how"
- Want multiple options
- Are learning something new
- Need to maintain precise control

Choose **Agent Mode** when you:

- Know exactly what you want
- Have repetitive changes
- Trust Copilot's implementation
- Want to save time on routine tasks

## Troubleshooting

**Chat not responding?**

- Check internet connection
- Verify Copilot subscription is active
- Try refreshing the chat window

**Code not compiling?**

- Ask Chat: "Fix the compilation errors in this code"
- Provide error messages to Chat for specific help

**Chat suggestions too complex?**

- Add "Keep it simple" or "Basic implementation" to prompts
- Ask for step-by-step explanations

**Agent Mode not available?**

- Ensure you have the latest Visual Studio update
- Check if your Copilot plan includes Agent Mode
- Try restarting Visual Studio

**Agent Mode making unwanted changes?**

- Use Ctrl+Z to undo
- Be more specific in your requests
- Switch to Ask Mode for more control
