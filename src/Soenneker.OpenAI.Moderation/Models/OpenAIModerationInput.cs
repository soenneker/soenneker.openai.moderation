using System;

namespace Soenneker.OpenAI.Moderation.Models;

/// <summary>
/// A text or image input for multimodal moderation.
/// </summary>
public sealed class OpenAIModerationInput
{
    /// <summary>
    /// The input type.
    /// </summary>
    public OpenAIModerationInputType Type { get; }

    /// <summary>
    /// The text content, image URL, or base64 data URL.
    /// </summary>
    public string Value { get; }

    private OpenAIModerationInput(OpenAIModerationInputType type, string value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// Creates a text moderation input.
    /// </summary>
    /// <param name="text">The text to moderate.</param>
    public static OpenAIModerationInput FromText(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        return new OpenAIModerationInput(OpenAIModerationInputType.Text, text);
    }

    /// <summary>
    /// Creates an image moderation input.
    /// </summary>
    /// <param name="imageUrl">An HTTP(S) image URL or a base64 data URL.</param>
    public static OpenAIModerationInput FromImageUrl(string imageUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(imageUrl);
        return new OpenAIModerationInput(OpenAIModerationInputType.ImageUrl, imageUrl);
    }

    /// <summary>
    /// Creates an image moderation input from a base64-encoded image.
    /// </summary>
    /// <param name="base64">The base64-encoded image payload, without a data URL prefix.</param>
    /// <param name="mediaType">The image MIME type, such as <c>image/png</c> or <c>image/jpeg</c>.</param>
    public static OpenAIModerationInput FromBase64Image(string base64, string mediaType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(base64);
        ArgumentException.ThrowIfNullOrWhiteSpace(mediaType);

        if (!mediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("The media type must be an image MIME type.", nameof(mediaType));

        return FromImageUrl($"data:{mediaType};base64,{base64}");
    }
}

/// <summary>
/// The supported multimodal moderation input types.
/// </summary>
public enum OpenAIModerationInputType
{
    /// <summary>
    /// Text content.
    /// </summary>
    Text,

    /// <summary>
    /// An image supplied by URL or base64 data URL.
    /// </summary>
    ImageUrl
}
