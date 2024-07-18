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
    private static readonly object Lock = new();
    private static bool _isServiceProviderInitialized = false;

    /// <summary>
    /// Static constructor to initialize service collection and service provider.
    /// </summary>
    static TestBase()
    {
        InitializeServiceProvider();
    }

    /// <summary>
    /// Initializes the service provider for the test cases.
    /// This method is called by the static constructor of the class.
    /// It creates a new service collection, adds necessary services to it,
    /// builds the service provider and initializes it.
    /// </summary>
    private static void InitializeServiceProvider()
    {
        // Lock the initialization to ensure only one instance of the service provider is created.
        lock (Lock)
        {
            if (!_isServiceProviderInitialized)
            {
                // Create a new service collection.
                var serviceCollection = new ServiceCollection();

                // Add logging to the service collection.
                serviceCollection.AddLogging();

                // Add the in-memory database to the service collection.
                serviceCollection.AddDbContext<LocalDbContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));

                // Add the update service to the service collection.
                serviceCollection.AddScoped(typeof(IUpdateService<>), typeof(UpdateService<>));

                // Add the http context accessor to the service collection.
                serviceCollection.AddHttpContextAccessor();

                // Add the http context accessor implementation to the service collection.
                serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                // Build the service provider from the service collection.
                var serviceProvider = serviceCollection.BuildServiceProvider();

                // Initialize the service provider.
                ServiceProviderHelper.Initialize(serviceProvider);

                // Mark the initialization as done.
                _isServiceProviderInitialized = true;
            }
        }
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
