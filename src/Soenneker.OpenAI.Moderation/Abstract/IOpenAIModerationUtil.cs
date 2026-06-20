using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.OpenAI.Moderation.Options;
using Soenneker.OpenAI.OpenApiClient.Models;

namespace Soenneker.OpenAI.Moderation.Abstract;

/// <summary>
/// A .NET OpenAI content moderation utility using their OpenAPI client
/// </summary>
public interface IOpenAIModerationUtil
{
    /// <summary>
    /// Moderates a single text input using options resolved from configuration.
    /// </summary>
    ValueTask<CreateModerationResponse?> Moderate(string input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates a single text input.
    /// </summary>
    ValueTask<CreateModerationResponse?> Moderate(string input, OpenAIModerationOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates multiple text inputs using options resolved from configuration.
    /// </summary>
    ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates multiple text inputs.
    /// </summary>
    ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs, OpenAIModerationOptions options, CancellationToken cancellationToken = default);
}
