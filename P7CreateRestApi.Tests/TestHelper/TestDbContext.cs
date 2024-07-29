using Dot.Net.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Tests;

/// <summary>
/// A test database context for simulating concurrency exceptions during unit tests.
/// </summary>
public class TestDbContext : LocalDbContext
{
    /// <summary>
    /// Gets or sets a value indicating whether to throw a DbUpdateConcurrencyException
    /// when SaveChangesAsync is called.
    /// </summary>
    public bool ThrowConcurrencyException { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public TestDbContext(DbContextOptions<LocalDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task{Int32}"/> that represents the asynchronous save operation.</returns>
    /// <exception cref="DbUpdateConcurrencyException">If ThrowConcurrencyException is set to true.</exception>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (ThrowConcurrencyException)
        {
            throw new DbUpdateConcurrencyException("Simulated concurrency exception");
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
