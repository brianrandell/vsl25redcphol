using BookLibraryAPI.Data;
using BookLibraryAPI.DTOs;
using BookLibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAPI.Services;

/// <summary>
/// Service implementation for book management
/// </summary>
public class BookService : IBookService
{
    private readonly LibraryContext _context;

    /// <summary>
    /// Initializes a new instance of the BookService
    /// </summary>
    /// <param name="context">Database context</param>
    public BookService(LibraryContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<PaginatedResultDto<BookDto>> GetAllBooksAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.Books.CountAsync(cancellationToken).ConfigureAwait(false);
        
        var books = await _context.Books
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => MapToDto(b))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PaginatedResultDto<BookDto>
        {
            Items = books,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc />
    public async Task<BookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return book != null ? MapToDto(book) : null;
    }

    /// <inheritdoc />
    public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto, CancellationToken cancellationToken = default)
    {
        var book = new Book
        {
            Title = createBookDto.Title,
            Author = createBookDto.Author,
            ISBN = createBookDto.ISBN,
            PublishedDate = createBookDto.PublishedDate,
            Genre = createBookDto.Genre,
            IsAvailable = createBookDto.IsAvailable,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToDto(book);
    }

    /// <inheritdoc />
    public async Task<BookDto?> UpdateBookAsync(int id, UpdateBookDto updateBookDto, CancellationToken cancellationToken = default)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (book == null)
            return null;

        book.Title = updateBookDto.Title;
        book.Author = updateBookDto.Author;
        book.ISBN = updateBookDto.ISBN;
        book.PublishedDate = updateBookDto.PublishedDate;
        book.Genre = updateBookDto.Genre;
        book.IsAvailable = updateBookDto.IsAvailable;
        book.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return MapToDto(book);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (book == null)
            return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    /// <inheritdoc />
    public async Task<PaginatedResultDto<BookDto>> SearchBooksAsync(BookSearchDto searchDto, CancellationToken cancellationToken = default)
    {
        var query = _context.Books.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchDto.Title))
        {
            query = query.Where(b => b.Title.ToLower().Contains(searchDto.Title.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(searchDto.Author))
        {
            query = query.Where(b => b.Author.ToLower().Contains(searchDto.Author.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(searchDto.Genre))
        {
            query = query.Where(b => b.Genre != null && b.Genre.ToLower().Contains(searchDto.Genre.ToLower()));
        }

        if (searchDto.IsAvailable.HasValue)
        {
            query = query.Where(b => b.IsAvailable == searchDto.IsAvailable.Value);
        }

        if (searchDto.PublishedDateFrom.HasValue)
        {
            query = query.Where(b => b.PublishedDate >= searchDto.PublishedDateFrom.Value);
        }

        if (searchDto.PublishedDateTo.HasValue)
        {
            query = query.Where(b => b.PublishedDate <= searchDto.PublishedDateTo.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var books = await query
            .OrderBy(b => b.Title)
            .Skip((searchDto.Page - 1) * searchDto.PageSize)
            .Take(searchDto.PageSize)
            .Select(b => MapToDto(b))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PaginatedResultDto<BookDto>
        {
            Items = books,
            TotalCount = totalCount,
            Page = searchDto.Page,
            PageSize = searchDto.PageSize
        };
    }

    /// <inheritdoc />
    public async Task<BookStatsDto> GetBookStatsAsync(CancellationToken cancellationToken = default)
    {
        var totalBooks = await _context.Books.CountAsync(cancellationToken).ConfigureAwait(false);
        var availableBooks = await _context.Books.CountAsync(b => b.IsAvailable, cancellationToken).ConfigureAwait(false);
        var checkedOutBooks = totalBooks - availableBooks;

        var booksByGenre = await _context.Books
            .Where(b => b.Genre != null)
            .GroupBy(b => b.Genre!)
            .Select(g => new { Genre = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Genre, x => x.Count, cancellationToken)
            .ConfigureAwait(false);

        var mostPopularAuthor = await _context.Books
            .GroupBy(b => b.Author)
            .Select(g => new { Author = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        var averageYear = await _context.Books
            .AverageAsync(b => b.PublishedDate.Year, cancellationToken)
            .ConfigureAwait(false);

        return new BookStatsDto
        {
            TotalBooks = totalBooks,
            AvailableBooks = availableBooks,
            CheckedOutBooks = checkedOutBooks,
            BooksByGenre = booksByGenre,
            MostPopularAuthor = mostPopularAuthor?.Author,
            MostPopularAuthorBookCount = mostPopularAuthor?.Count ?? 0,
            AveragePublicationYear = averageYear
        };
    }

    /// <summary>
    /// Maps a Book entity to a BookDto
    /// </summary>
    /// <param name="book">Book entity</param>
    /// <returns>BookDto</returns>
    private static BookDto MapToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            PublishedDate = book.PublishedDate,
            Genre = book.Genre,
            IsAvailable = book.IsAvailable,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        };
    }
}