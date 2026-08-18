using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.OpenAI.Moderation.Models;
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

    /// <summary>
    /// Moderates an image using options resolved from configuration.
    /// </summary>
    /// <param name="imageUrl">An HTTP(S) image URL or a base64 data URL.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled.</returns>
    ValueTask<CreateModerationResponse?> ModerateImage(string imageUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates an image.
    /// </summary>
    /// <param name="imageUrl">An HTTP(S) image URL or a base64 data URL.</param>
    /// <param name="options">The moderation options to apply.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled.</returns>
    ValueTask<CreateModerationResponse?> ModerateImage(string imageUrl, OpenAIModerationOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates a base64-encoded image using options resolved from configuration.
    /// </summary>
    /// <param name="base64">The base64-encoded image payload, without a data URL prefix.</param>
    /// <param name="mediaType">The image MIME type, such as <c>image/png</c> or <c>image/jpeg</c>.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled.</returns>
    ValueTask<CreateModerationResponse?> ModerateBase64Image(string base64, string mediaType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates a base64-encoded image.
    /// </summary>
    /// <param name="base64">The base64-encoded image payload, without a data URL prefix.</param>
    /// <param name="mediaType">The image MIME type, such as <c>image/png</c> or <c>image/jpeg</c>.</param>
    /// <param name="options">The moderation options to apply.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled.</returns>
    ValueTask<CreateModerationResponse?> ModerateBase64Image(string base64, string mediaType, OpenAIModerationOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates a combined collection of text and image inputs using options resolved from configuration.
    /// </summary>
    /// <param name="inputs">The multimodal inputs to moderate together.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled or no inputs are supplied.</returns>
    ValueTask<CreateModerationResponse?> ModerateMultimodal(IReadOnlyList<OpenAIModerationInput> inputs, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates a combined collection of text and image inputs.
    /// </summary>
    /// <param name="inputs">The multimodal inputs to moderate together.</param>
    /// <param name="options">The moderation options to apply.</param>
    /// <param name="cancellationToken">A token to cancel the moderation request.</param>
    /// <returns>The moderation response, or <see langword="null"/> when moderation is disabled or no inputs are supplied.</returns>
    ValueTask<CreateModerationResponse?> ModerateMultimodal(IReadOnlyList<OpenAIModerationInput> inputs, OpenAIModerationOptions options,
        CancellationToken cancellationToken = default);
}
