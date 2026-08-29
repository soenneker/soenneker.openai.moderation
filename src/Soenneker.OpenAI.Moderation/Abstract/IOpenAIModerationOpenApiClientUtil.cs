using System.Threading;
using System.Threading.Tasks;
using Soenneker.OpenAI.OpenApiClient;

namespace Soenneker.OpenAI.Moderation.Abstract;

/// <summary>
/// Provides the OpenAI client configured specifically for moderation requests.
/// </summary>
public interface IOpenAIModerationOpenApiClientUtil
{
    /// <summary>
    /// Gets the configured OpenAI client.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested openAI Open API Client.</returns>
    ValueTask<OpenAIOpenApiClient> Get(CancellationToken cancellationToken = default);
}
