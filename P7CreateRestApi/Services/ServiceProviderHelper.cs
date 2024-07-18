namespace Dot.Net.WebApi.Helpers
{
    /// <summary>
    /// Helper class for managing the ServiceProvider.
    /// </summary>
    public static class ServiceProviderHelper
    {
        private static IServiceProvider _serviceProvider;
        private static readonly object Lock = new();

        /// <summary>
        /// Gets or sets the ServiceProvider instance.
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    throw new InvalidOperationException("ServiceProvider has not been initialized.");
                }
                return _serviceProvider;
            }
        }

        /// <summary>
        /// Gets a service of type T from the ServiceProvider.
        /// </summary>
        /// <typeparam name="T">The type of service to retrieve.</typeparam>
        /// <returns>The service of type T.</returns>
        public static T GetService<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }

        /// <summary>
        /// Initializes the ServiceProvider instance.
        /// </summary>
        /// <param name="serviceProvider">The ServiceProvider instance to set.</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            lock (Lock)
            {
                _serviceProvider = serviceProvider;
            }
        }
    }
}