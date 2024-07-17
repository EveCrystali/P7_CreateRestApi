using System;

using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Moq;
using Dot.Net.WebApi.Helpers;

namespace P7CreateRestApi.Tests;

/// <summary>
/// Base test class for entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class TestBase<TEntity> : IDisposable where TEntity : class
{
    protected readonly LocalDbContext _context;
    protected readonly Mock<IUpdateService<TEntity>> _mockUpdateService;
    protected readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Static constructor to initialize service collection and service provider.
    /// </summary>
    static TestBase()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddDbContext<LocalDbContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));
        serviceCollection.AddScoped(typeof(IUpdateService<>), typeof(UpdateService<>));
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        ServiceProviderHelper.Initialize(serviceProvider);
    }

    private bool disposed = false;

    /// <summary>
    /// Dispose method to release resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose method to release resources based on disposing flag.
    /// </summary>
    /// <param name="disposing">Flag to indicate if disposing is in progress.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Release managed resources
                _context.Dispose();
            }

            // Release unmanaged resources

            disposed = true;
        }
    }

    /// <summary>
    /// Destructor to dispose resources.
    /// </summary>
    ~TestBase()
    {
        Dispose(false);
    }

    /// <summary>
    /// Constructor to initialize test base.
    /// </summary>
    protected TestBase()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
                      .UseInMemoryDatabase("TestDatabase")
                      .Options;

        _context = new LocalDbContext(options);
        _mockUpdateService = new Mock<IUpdateService<TEntity>>();

        var httpContextAccessor = new HttpContextAccessor();

        // Simulate an HTTP context
        var context = new DefaultHttpContext();
        httpContextAccessor.HttpContext = context;

        // Reset the database (useful to run tests in parallel)
        _context.Set<TEntity>().RemoveRange(_context.Set<TEntity>());
        _context.SaveChanges();

        _serviceProvider = ServiceProviderHelper.ServiceProvider;
    }
}
