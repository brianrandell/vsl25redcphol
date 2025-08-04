using System.ComponentModel.DataAnnotations;

namespace BookLibraryAPI.Models;

/// <summary>
/// Represents a book in the library system
/// </summary>
public class Book
{
    /// <summary>
    /// Unique identifier for the book
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the book (required)
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Author of the book (required)
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// International Standard Book Number
    /// </summary>
    [StringLength(20)]
    public string? ISBN { get; set; }

    /// <summary>
    /// Date the book was published
    /// </summary>
    public DateTime PublishedDate { get; set; }

    /// <summary>
    /// Genre/category of the book
    /// </summary>
    [StringLength(50)]
    public string? Genre { get; set; }

    /// <summary>
    /// Whether the book is currently available for checkout
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Date when the book was added to the library
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the book record was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}