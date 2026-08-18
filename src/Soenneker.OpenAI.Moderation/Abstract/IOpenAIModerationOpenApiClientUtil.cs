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
    ValueTask<OpenAIOpenApiClient> Get(CancellationToken cancellationToken = default);
}
