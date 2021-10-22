using ApiWrapper.SpotifyServiceClient;
using Microsoft.Extensions.DependencyInjection;

namespace ApiWrapper.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApiWrapper(this IServiceCollection services)
        {
            services.AddScoped<ISpotifyClient, SpotifyClient>();

            return services;
        }
    }
}