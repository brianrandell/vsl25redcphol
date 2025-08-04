namespace BookLibraryAPI.DTOs;

/// <summary>
/// Data transfer object for book statistics
/// </summary>
public record BookStatsDto
{
    /// <summary>
    /// Total number of books in the library
    /// </summary>
    public int TotalBooks { get; init; }

    /// <summary>
    /// Number of books currently available
    /// </summary>
    public int AvailableBooks { get; init; }

    /// <summary>
    /// Number of books currently checked out
    /// </summary>
    public int CheckedOutBooks { get; init; }

    /// <summary>
    /// Books grouped by genre with counts
    /// </summary>
    public Dictionary<string, int> BooksByGenre { get; init; } = new();

    /// <summary>
    /// Most popular author (author with most books)
    /// </summary>
    public string? MostPopularAuthor { get; init; }

    /// <summary>
    /// Number of books by the most popular author
    /// </summary>
    public int MostPopularAuthorBookCount { get; init; }

    /// <summary>
    /// Average publication year of all books
    /// </summary>
    public double AveragePublicationYear { get; init; }

    /// <summary>
    /// Date and time when these statistics were generated
    /// </summary>
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
}