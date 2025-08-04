using BookLibraryAPI.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace BookLibraryAPI.Tests;

/// <summary>
/// Integration tests for the Books API controller
/// </summary>
public class BooksControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the BooksControllerTests
    /// </summary>
    /// <param name="factory">Web application factory</param>
    public BooksControllerTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <summary>
    /// Tests getting all books returns paginated results
    /// </summary>
    [Fact]
    public async Task GetBooks_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/books");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Be("application/json; charset=utf-8");

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaginatedResultDto<BookDto>>(content, _jsonOptions);

        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();
        result.TotalCount.Should().BeGreaterThan(0);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    /// <summary>
    /// Tests getting books with pagination parameters
    /// </summary>
    [Fact]
    public async Task GetBooks_WithPagination_ReturnsCorrectPage()
    {
        // Act
        var response = await _client.GetAsync("/api/books?page=1&pageSize=5");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaginatedResultDto<BookDto>>(content, _jsonOptions);

        result.Should().NotBeNull();
        result!.Page.Should().Be(1);
        result.PageSize.Should().Be(5);
        result.Items.Should().HaveCountLessThanOrEqualTo(5);
    }

    /// <summary>
    /// Tests getting a specific book by ID
    /// </summary>
    [Fact]
    public async Task GetBook_WithValidId_ReturnsBook()
    {
        // Arrange - Get first book to use its ID
        var booksResponse = await _client.GetAsync("/api/books");
        var booksContent = await booksResponse.Content.ReadAsStringAsync();
        var booksResult = JsonSerializer.Deserialize<PaginatedResultDto<BookDto>>(booksContent, _jsonOptions);
        var firstBook = booksResult!.Items.First();

        // Act
        var response = await _client.GetAsync($"/api/books/{firstBook.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var book = JsonSerializer.Deserialize<BookDto>(content, _jsonOptions);

        book.Should().NotBeNull();
        book!.Id.Should().Be(firstBook.Id);
        book.Title.Should().NotBeEmpty();
        book.Author.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests getting a non-existent book returns 404
    /// </summary>
    [Fact]
    public async Task GetBook_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/books/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests creating a new book
    /// </summary>
    [Fact]
    public async Task CreateBook_WithValidData_ReturnsCreatedBook()
    {
        // Arrange
        var newBook = new CreateBookDto
        {
            Title = "Test Book",
            Author = "Test Author",
            ISBN = "978-1-234-56789-0", // Unique ISBN different from seed data
            PublishedDate = new DateTime(2023, 1, 1),
            Genre = "Test Genre",
            IsAvailable = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", newBook, _jsonOptions);

        // Assert
        if (response.StatusCode != HttpStatusCode.Created)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error response: {errorContent}");
        }
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        var createdBook = JsonSerializer.Deserialize<BookDto>(content, _jsonOptions);

        createdBook.Should().NotBeNull();
        createdBook!.Title.Should().Be(newBook.Title);
        createdBook.Author.Should().Be(newBook.Author);
        createdBook.ISBN.Should().Be(newBook.ISBN);
        createdBook.Genre.Should().Be(newBook.Genre);
        createdBook.IsAvailable.Should().Be(newBook.IsAvailable);
        createdBook.Id.Should().BeGreaterThan(0);

        // Verify Location header
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().Contain($"/api/Books/{createdBook.Id}");
    }

    /// <summary>
    /// Tests creating a book with invalid data returns validation errors
    /// </summary>
    [Fact]
    public async Task CreateBook_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange - Missing required Title
        var invalidBook = new CreateBookDto
        {
            Title = "", // Invalid - required
            Author = "Test Author",
            PublishedDate = new DateTime(2023, 1, 1)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", invalidBook, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Tests updating an existing book
    /// </summary>
    [Fact]
    public async Task UpdateBook_WithValidData_ReturnsUpdatedBook()
    {
        // Arrange - Create a book first
        var newBook = new CreateBookDto
        {
            Title = "Original Title",
            Author = "Original Author",
            PublishedDate = new DateTime(2023, 1, 1),
            Genre = "Original Genre"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/books", newBook, _jsonOptions);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdBook = JsonSerializer.Deserialize<BookDto>(createContent, _jsonOptions);

        var updateBook = new UpdateBookDto
        {
            Title = "Updated Title",
            Author = "Updated Author",
            PublishedDate = new DateTime(2023, 6, 1),
            Genre = "Updated Genre",
            IsAvailable = false
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/books/{createdBook!.Id}", updateBook, _jsonOptions);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var updatedBookResult = JsonSerializer.Deserialize<BookDto>(content, _jsonOptions);

        updatedBookResult.Should().NotBeNull();
        updatedBookResult!.Id.Should().Be(createdBook.Id);
        updatedBookResult.Title.Should().Be(updateBook.Title);
        updatedBookResult.Author.Should().Be(updateBook.Author);
        updatedBookResult.Genre.Should().Be(updateBook.Genre);
        updatedBookResult.IsAvailable.Should().Be(updateBook.IsAvailable);
    }

    /// <summary>
    /// Tests updating a non-existent book returns 404
    /// </summary>
    [Fact]
    public async Task UpdateBook_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var updateBook = new UpdateBookDto
        {
            Title = "Updated Title",
            Author = "Updated Author",
            PublishedDate = new DateTime(2023, 1, 1)
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/books/99999", updateBook, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests deleting an existing book
    /// </summary>
    [Fact]
    public async Task DeleteBook_WithValidId_ReturnsNoContent()
    {
        // Arrange - Create a book first
        var newBook = new CreateBookDto
        {
            Title = "Book to Delete",
            Author = "Delete Author",
            PublishedDate = new DateTime(2023, 1, 1)
        };

        var createResponse = await _client.PostAsJsonAsync("/api/books", newBook, _jsonOptions);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdBook = JsonSerializer.Deserialize<BookDto>(createContent, _jsonOptions);

        // Act
        var response = await _client.DeleteAsync($"/api/books/{createdBook!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify book is actually deleted
        var getResponse = await _client.GetAsync($"/api/books/{createdBook.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests deleting a non-existent book returns 404
    /// </summary>
    [Fact]
    public async Task DeleteBook_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/books/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests searching books with various criteria
    /// </summary>
    [Fact]
    public async Task SearchBooks_WithTitle_ReturnsMatchingBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/books/search?title=great");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaginatedResultDto<BookDto>>(content, _jsonOptions);

        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();
        result.Items.Should().OnlyContain(book => book.Title.ToLower().Contains("great"));
    }

    /// <summary>
    /// Tests searching books by author
    /// </summary>
    [Fact]
    public async Task SearchBooks_WithAuthor_ReturnsMatchingBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/books/search?author=fitzgerald");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaginatedResultDto<BookDto>>(content, _jsonOptions);

        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();
        result.Items.Should().OnlyContain(book => book.Author.ToLower().Contains("fitzgerald"));
    }

    /// <summary>
    /// Tests searching books by availability
    /// </summary>
    [Fact]
    public async Task SearchBooks_WithAvailability_ReturnsMatchingBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/books/search?isAvailable=true");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaginatedResultDto<BookDto>>(content, _jsonOptions);

        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();
        result.Items.Should().OnlyContain(book => book.IsAvailable == true);
    }

    /// <summary>
    /// Tests searching books with pagination
    /// </summary>
    [Fact]
    public async Task SearchBooks_WithPagination_ReturnsCorrectPage()
    {
        // Act
        var response = await _client.GetAsync("/api/books/search?page=1&pageSize=3");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PaginatedResultDto<BookDto>>(content, _jsonOptions);

        result.Should().NotBeNull();
        result!.Page.Should().Be(1);
        result.PageSize.Should().Be(3);
        result.Items.Should().HaveCountLessThanOrEqualTo(3);
    }

    /// <summary>
    /// Tests getting book statistics
    /// </summary>
    [Fact]
    public async Task GetBookStats_ReturnsValidStatistics()
    {
        // Act
        var response = await _client.GetAsync("/api/books/stats");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var stats = JsonSerializer.Deserialize<BookStatsDto>(content, _jsonOptions);

        stats.Should().NotBeNull();
        stats!.TotalBooks.Should().BeGreaterThan(0);
        stats.AvailableBooks.Should().BeGreaterThanOrEqualTo(0);
        stats.CheckedOutBooks.Should().BeGreaterThanOrEqualTo(0);
        stats.TotalBooks.Should().Be(stats.AvailableBooks + stats.CheckedOutBooks);
        stats.BooksByGenre.Should().NotBeEmpty();
        stats.MostPopularAuthor.Should().NotBeNullOrEmpty();
        stats.MostPopularAuthorBookCount.Should().BeGreaterThan(0);
        stats.AveragePublicationYear.Should().BeGreaterThan(1800);
        stats.GeneratedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Tests that API handles concurrent requests properly
    /// </summary>
    [Fact]
    public async Task GetBooks_ConcurrentRequests_AllSucceed()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();

        // Act - Make 10 concurrent requests
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/api/books"));
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().HaveCount(10);
        responses.Should().OnlyContain(response => response.IsSuccessStatusCode);
    }

    /// <summary>
    /// Tests API returns proper error format for validation errors
    /// </summary>
    [Fact]
    public async Task CreateBook_WithLongTitle_ReturnsValidationError()
    {
        // Arrange - Title exceeds max length
        var invalidBook = new CreateBookDto
        {
            Title = new string('A', 201), // Exceeds 200 character limit
            Author = "Test Author",
            PublishedDate = new DateTime(2023, 1, 1)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", invalidBook, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }
}