using Soenneker.Gen.EnumValues;

namespace Soenneker.OpenAI.Moderation.Enums;

/// <summary>
/// OpenAI moderation category names.
/// </summary>
[EnumValue<string>]
public sealed partial class OpenAIModerationCategoryNames
{
    /// <summary>
    /// Harassment category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames Harassment = new("harassment");

    /// <summary>
    /// Harassment with threatening content category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames HarassmentThreatening = new("harassment/threatening");

    /// <summary>
    /// Hate category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames Hate = new("hate");

    /// <summary>
    /// Hate with threatening content category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames HateThreatening = new("hate/threatening");

    /// <summary>
    /// Illicit content category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames Illicit = new("illicit");

    /// <summary>
    /// Illicit violent content category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames IllicitViolent = new("illicit/violent");

    /// <summary>
    /// Self-harm category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames SelfHarm = new("self-harm");

    /// <summary>
    /// Self-harm intent category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames SelfHarmIntent = new("self-harm/intent");

    /// <summary>
    /// Self-harm instructions category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames SelfHarmInstructions = new("self-harm/instructions");

    /// <summary>
    /// Sexual content category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames Sexual = new("sexual");

    /// <summary>
    /// Sexual content involving minors category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames SexualMinors = new("sexual/minors");

    /// <summary>
    /// Violence category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames Violence = new("violence");

    /// <summary>
    /// Graphic violence category.
    /// </summary>
    public static readonly OpenAIModerationCategoryNames ViolenceGraphic = new("violence/graphic");
}
