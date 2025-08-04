using BookLibraryAPI.DTOs;
using BookLibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryAPI.Controllers;

/// <summary>
/// Controller for managing books in the library system
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    /// <summary>
    /// Initializes a new instance of the BooksController
    /// </summary>
    /// <param name="bookService">Book service</param>
    /// <param name="logger">Logger</param>
    public BooksController(IBookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all books with optional pagination
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10, max: 100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of books</returns>
    /// <response code="200">Returns the paginated list of books</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResultDto<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResultDto<BookDto>>> GetBooks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            _logger.LogInformation("Getting books - Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var result = await _bookService.GetAllBooksAsync(page, pageSize, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting books");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving books");
        }
    }

    /// <summary>
    /// Gets a specific book by ID
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Book details</returns>
    /// <response code="200">Returns the book</response>
    /// <response code="404">Book not found</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> GetBook(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting book with ID: {BookId}", id);

            var book = await _bookService.GetBookByIdAsync(id, cancellationToken).ConfigureAwait(false);
            
            if (book == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found", id);
                return NotFound($"Book with ID {id} not found");
            }

            return Ok(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting book with ID: {BookId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the book");
        }
    }

    /// <summary>
    /// Creates a new book
    /// </summary>
    /// <param name="createBookDto">Book creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created book</returns>
    /// <response code="201">Book created successfully</response>
    /// <response code="400">Invalid book data</response>
    [HttpPost]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> CreateBook(
        [FromBody] CreateBookDto createBookDto, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new book: {Title} by {Author}", createBookDto.Title, createBookDto.Author);

            var createdBook = await _bookService.CreateBookAsync(createBookDto, cancellationToken).ConfigureAwait(false);
            
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating book: {Title}", createBookDto.Title);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the book");
        }
    }

    /// <summary>
    /// Updates an existing book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="updateBookDto">Book update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated book</returns>
    /// <response code="200">Book updated successfully</response>
    /// <response code="400">Invalid book data</response>
    /// <response code="404">Book not found</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> UpdateBook(
        int id, 
        [FromBody] UpdateBookDto updateBookDto, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating book with ID: {BookId}", id);

            var updatedBook = await _bookService.UpdateBookAsync(id, updateBookDto, cancellationToken).ConfigureAwait(false);
            
            if (updatedBook == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found for update", id);
                return NotFound($"Book with ID {id} not found");
            }

            return Ok(updatedBook);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating book with ID: {BookId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the book");
        }
    }

    /// <summary>
    /// Deletes a book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">Book deleted successfully</response>
    /// <response code="404">Book not found</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting book with ID: {BookId}", id);

            var deleted = await _bookService.DeleteBookAsync(id, cancellationToken).ConfigureAwait(false);
            
            if (!deleted)
            {
                _logger.LogWarning("Book with ID {BookId} not found for deletion", id);
                return NotFound($"Book with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting book with ID: {BookId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the book");
        }
    }

    /// <summary>
    /// Searches books based on various criteria with pagination
    /// </summary>
    /// <param name="title">Title search term (partial, case-insensitive)</param>
    /// <param name="author">Author search term (partial, case-insensitive)</param>
    /// <param name="genre">Genre filter</param>
    /// <param name="isAvailable">Availability filter</param>
    /// <param name="publishedDateFrom">Published date from (inclusive)</param>
    /// <param name="publishedDateTo">Published date to (inclusive)</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Number of items per page (default: 10, max: 100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated search results</returns>
    /// <response code="200">Returns the search results</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PaginatedResultDto<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResultDto<BookDto>>> SearchBooks(
        [FromQuery] string? title = null,
        [FromQuery] string? author = null,
        [FromQuery] string? genre = null,
        [FromQuery] bool? isAvailable = null,
        [FromQuery] DateTime? publishedDateFrom = null,
        [FromQuery] DateTime? publishedDateTo = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var searchDto = new BookSearchDto
            {
                Title = title,
                Author = author,
                Genre = genre,
                IsAvailable = isAvailable,
                PublishedDateFrom = publishedDateFrom,
                PublishedDateTo = publishedDateTo,
                Page = page,
                PageSize = pageSize
            };

            _logger.LogInformation("Searching books with criteria: Title={Title}, Author={Author}, Genre={Genre}, Available={Available}",
                title, author, genre, isAvailable);

            var result = await _bookService.SearchBooksAsync(searchDto, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching books");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching books");
        }
    }

    /// <summary>
    /// Gets library statistics including total books, availability, genres, and popular authors
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Library statistics</returns>
    /// <response code="200">Returns the library statistics</response>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(BookStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BookStatsDto>> GetBookStats(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting library statistics");

            var stats = await _bookService.GetBookStatsAsync(cancellationToken).ConfigureAwait(false);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting book statistics");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving statistics");
        }
    }
}