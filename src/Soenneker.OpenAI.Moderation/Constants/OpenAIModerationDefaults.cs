namespace Soenneker.OpenAI.Moderation.Constants;

/// <summary>
/// Defaults for OpenAI moderation requests.
/// </summary>
public static class OpenAIModerationDefaults
{
    public const string Model = "omni-moderation-latest";
    public const string EnabledConfigurationKey = "OpenAI:Moderation:Enabled";
    public const string ModelConfigurationKey = "OpenAI:Moderation:Model";
    public const string ApiKeyConfigurationKey = "OpenAI:ApiKey";
}
