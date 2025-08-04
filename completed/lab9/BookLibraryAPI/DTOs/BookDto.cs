using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.DTOs;

/// <summary>
/// Data transfer object for book responses
/// </summary>
public record BookDto
{
    /// <summary>
    /// Unique identifier for the book
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Title of the book
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Author of the book
    /// </summary>
    public string Author { get; init; } = string.Empty;

    /// <summary>
    /// International Standard Book Number
    /// </summary>
    public string? ISBN { get; init; }

    /// <summary>
    /// Date the book was published
    /// </summary>
    public DateTime PublishedDate { get; init; }

    /// <summary>
    /// Genre/category of the book
    /// </summary>
    public string? Genre { get; init; }

    /// <summary>
    /// Whether the book is currently available for checkout
    /// </summary>
    public bool IsAvailable { get; init; }

    /// <summary>
    /// Date when the book was added to the library
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Date when the book record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; init; }
}