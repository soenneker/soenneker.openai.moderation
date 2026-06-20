using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.OpenAI.Moderation.Abstract;
using Soenneker.OpenAI.OpenApiClientUtil.Registrars;

namespace Soenneker.OpenAI.Moderation.Registrars;

/// <summary>
/// A .NET OpenAI content moderation utility using their OpenAPI client
/// </summary>
public static class OpenAIModerationUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IOpenAIModerationUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddOpenAIModerationUtilAsSingleton(this IServiceCollection services)
    {
        services.AddOpenAIOpenApiClientUtilAsSingleton()
                .TryAddSingleton<IOpenAIModerationUtil, OpenAIModerationUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IOpenAIModerationUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddOpenAIModerationUtilAsScoped(this IServiceCollection services)
    {
        services.AddOpenAIOpenApiClientUtilAsScoped()
                .TryAddScoped<IOpenAIModerationUtil, OpenAIModerationUtil>();

        return services;
    }
}
