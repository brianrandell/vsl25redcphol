using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.DTOs;

/// <summary>
/// Data transfer object for updating an existing book
/// </summary>
public record UpdateBookDto
{
    /// <summary>
    /// Title of the book (required)
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Author of the book (required)
    /// </summary>
    [Required(ErrorMessage = "Author is required")]
    [StringLength(100, ErrorMessage = "Author must not exceed 100 characters")]
    public string Author { get; init; } = string.Empty;

    /// <summary>
    /// International Standard Book Number
    /// </summary>
    [StringLength(20, ErrorMessage = "ISBN must not exceed 20 characters")]
    public string? ISBN { get; init; }

    /// <summary>
    /// Date the book was published
    /// </summary>
    public DateTime PublishedDate { get; init; }

    /// <summary>
    /// Genre/category of the book
    /// </summary>
    [StringLength(50, ErrorMessage = "Genre must not exceed 50 characters")]
    public string? Genre { get; init; }

    /// <summary>
    /// Whether the book is currently available for checkout
    /// </summary>
    public bool IsAvailable { get; init; }
}