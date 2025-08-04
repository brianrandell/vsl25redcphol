namespace BookLibraryAPI.DTOs;

/// <summary>
/// Data transfer object for book search parameters
/// </summary>
public record BookSearchDto
{
    /// <summary>
    /// Search term for title (case-insensitive)
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Search term for author (partial match)
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// Filter by genre
    /// </summary>
    public string? Genre { get; init; }

    /// <summary>
    /// Filter by availability status (null = all, true = available only, false = unavailable only)
    /// </summary>
    public bool? IsAvailable { get; init; }

    /// <summary>
    /// Filter by published date from (inclusive)
    /// </summary>
    public DateTime? PublishedDateFrom { get; init; }

    /// <summary>
    /// Filter by published date to (inclusive)
    /// </summary>
    public DateTime? PublishedDateTo { get; init; }

    /// <summary>
    /// Page number for pagination (1-based)
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; init; } = 10;
}