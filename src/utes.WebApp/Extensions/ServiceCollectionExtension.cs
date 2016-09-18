using Microsoft.Extensions.DependencyInjection;
using utes.Interfaces;

namespace utes.WebApp.Extensions
{
    /// <summary>
    /// Class to bundle the interfaces until implemente auto discovery.
    /// </summary>
    internal static class ServiceCollectionExtension
    {
        /// <summary>
        /// Extension method to bundle the dependencies of the web app.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection with the dependencies added.</returns>
        internal static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IAssemblyStorage, WebApplicationAssemblyStorage.WebApplicationAssemblyStorage>();

            return services;
        }
    }
}
