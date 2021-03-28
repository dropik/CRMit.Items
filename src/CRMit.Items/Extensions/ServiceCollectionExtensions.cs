using CRMit.Items.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CRMit.Items.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartupTask
            => services.AddTransient<IStartupTask, T>();
    }
}
