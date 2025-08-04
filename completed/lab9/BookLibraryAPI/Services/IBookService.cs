using BookLibraryAPI.DTOs;

namespace BookLibraryAPI.Services;

/// <summary>
/// Interface for book management services
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Gets all books with optional pagination
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of books</returns>
    Task<PaginatedResultDto<BookDto>> GetAllBooksAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a book by its ID
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Book if found, null otherwise</returns>
    Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new book
    /// </summary>
    /// <param name="createBookDto">Book creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created book</returns>
    Task<BookDto> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="updateBookDto">Book update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated book if found, null otherwise</returns>
    Task<BookDto?> UpdateBookAsync(int id, UpdateBookDto updateBookDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches books based on provided criteria
    /// </summary>
    /// <param name="searchDto">Search criteria</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated search results</returns>
    Task<PaginatedResultDto<BookDto>> SearchBooksAsync(BookSearchDto searchDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets library statistics
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Library statistics</returns>
    Task<BookStatsDto> GetBookStatsAsync(CancellationToken cancellationToken = default);
}