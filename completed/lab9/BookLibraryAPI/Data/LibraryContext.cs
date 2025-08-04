using BookLibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAPI.Data;

/// <summary>
/// Entity Framework database context for the Book Library system
/// </summary>
public class LibraryContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the LibraryContext
    /// </summary>
    /// <param name="options">Database context options</param>
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    /// <summary>
    /// Books in the library
    /// </summary>
    public DbSet<Book> Books { get; set; } = null!;

    /// <summary>
    /// Configures the model relationships and constraints
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(e => e.Author)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.ISBN)
                .HasMaxLength(20);
                
            entity.Property(e => e.Genre)
                .HasMaxLength(50);
                
            entity.HasIndex(e => e.ISBN)
                .IsUnique()
                .HasFilter("[ISBN] IS NOT NULL");
                
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Author);
            entity.HasIndex(e => e.Genre);
            entity.HasIndex(e => e.IsAvailable);
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Seeds the database with initial sample data
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        var baseDate = DateTime.UtcNow.AddDays(-30);

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                ISBN = "978-0-7432-7356-5",
                PublishedDate = new DateTime(1925, 4, 10),
                Genre = "Fiction",
                IsAvailable = true,
                CreatedAt = baseDate,
                UpdatedAt = baseDate
            },
            new Book
            {
                Id = 2,
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                ISBN = "978-0-06-112008-4",
                PublishedDate = new DateTime(1960, 7, 11),
                Genre = "Fiction",
                IsAvailable = true,
                CreatedAt = baseDate.AddDays(1),
                UpdatedAt = baseDate.AddDays(1)
            },
            new Book
            {
                Id = 3,
                Title = "1984",
                Author = "George Orwell",
                ISBN = "978-0-452-28423-4",
                PublishedDate = new DateTime(1949, 6, 8),
                Genre = "Dystopian Fiction",
                IsAvailable = false,
                CreatedAt = baseDate.AddDays(2),
                UpdatedAt = baseDate.AddDays(2)
            },
            new Book
            {
                Id = 4,
                Title = "Clean Code",
                Author = "Robert C. Martin",
                ISBN = "978-0-13-235088-4",
                PublishedDate = new DateTime(2008, 8, 1),
                Genre = "Technology",
                IsAvailable = true,
                CreatedAt = baseDate.AddDays(3),
                UpdatedAt = baseDate.AddDays(3)
            },
            new Book
            {
                Id = 5,
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt",
                ISBN = "978-0-201-61622-4",
                PublishedDate = new DateTime(1999, 10, 20),
                Genre = "Technology",
                IsAvailable = true,
                CreatedAt = baseDate.AddDays(4),
                UpdatedAt = baseDate.AddDays(4)
            },
            new Book
            {
                Id = 6,
                Title = "Design Patterns",
                Author = "Gang of Four",
                ISBN = "978-0-201-63361-0",
                PublishedDate = new DateTime(1994, 10, 21),
                Genre = "Technology",
                IsAvailable = false,
                CreatedAt = baseDate.AddDays(5),
                UpdatedAt = baseDate.AddDays(5)
            },
            new Book
            {
                Id = 7,
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                ISBN = "978-0-14-143951-8",
                PublishedDate = new DateTime(1813, 1, 28),
                Genre = "Romance",
                IsAvailable = true,
                CreatedAt = baseDate.AddDays(6),
                UpdatedAt = baseDate.AddDays(6)
            },
            new Book
            {
                Id = 8,
                Title = "The Catcher in the Rye",
                Author = "J.D. Salinger",
                ISBN = "978-0-316-76948-0",
                PublishedDate = new DateTime(1951, 7, 16),
                Genre = "Fiction",
                IsAvailable = true,
                CreatedAt = baseDate.AddDays(7),
                UpdatedAt = baseDate.AddDays(7)
            }
        );
    }

    /// <summary>
    /// Override SaveChanges to automatically update UpdatedAt timestamp
    /// </summary>
    /// <returns>Number of affected records</returns>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to automatically update UpdatedAt timestamp
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates timestamps for modified entities
    /// </summary>
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<Book>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}