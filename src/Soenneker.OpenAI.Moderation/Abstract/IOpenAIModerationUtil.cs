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
    /// <param name="input">The text input to moderate.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled or the input is empty.</returns>
    ValueTask<CreateModerationResponse?> Moderate(string input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates a single text input.
    /// </summary>
    /// <param name="input">The text input to moderate.</param>
    /// <param name="options">The moderation options to apply.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled or the input is empty.</returns>
    ValueTask<CreateModerationResponse?> Moderate(string input, OpenAIModerationOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates multiple text inputs using options resolved from configuration.
    /// </summary>
    /// <param name="inputs">The text inputs to moderate.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled or all inputs are empty.</returns>
    ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates multiple text inputs.
    /// </summary>
    /// <param name="inputs">The text inputs to moderate.</param>
    /// <param name="options">The moderation options to apply.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled or all inputs are empty.</returns>
    ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs, OpenAIModerationOptions options, CancellationToken cancellationToken = default);
}
