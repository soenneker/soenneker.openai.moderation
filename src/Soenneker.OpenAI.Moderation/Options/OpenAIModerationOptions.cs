using System;
using Microsoft.Extensions.Configuration;
using Soenneker.OpenAI.Moderation.Constants;

namespace Soenneker.OpenAI.Moderation.Options;

/// <summary>
/// Options for OpenAI moderation requests.
/// </summary>
public sealed class OpenAIModerationOptions
{
    /// <summary>
    /// Whether moderation should be performed.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// OpenAI moderation model to use.
    /// </summary>
    public string Model { get; set; } = OpenAIModerationDefaults.Model;

    /// <summary>
    /// Creates options from conventional OpenAI configuration keys.
    /// </summary>
    /// <param name="configuration">The configuration to read moderation settings from.</param>
    /// <returns>A new options instance populated from configuration.</returns>
    public static OpenAIModerationOptions FromConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var options = new OpenAIModerationOptions();

        string? enabled = configuration[OpenAIModerationDefaults.EnabledConfigurationKey];

        if (bool.TryParse(enabled, out bool parsedEnabled))
            options.Enabled = parsedEnabled;

        string? model = configuration[OpenAIModerationDefaults.ModelConfigurationKey];

        if (!string.IsNullOrWhiteSpace(model))
            options.Model = model;

        return options;
    }
}
