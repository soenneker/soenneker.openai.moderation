using System.Collections.Generic;
using Soenneker.Gen.EnumValues;

namespace Soenneker.OpenAI.Moderation.Constants;

/// <summary>
/// OpenAI moderation category names.
/// </summary>
[EnumValue<string>]
public sealed partial class OpenAIModerationCategoryNames
{
    public static readonly OpenAIModerationCategoryNames Harassment = new("harassment");

    public static readonly OpenAIModerationCategoryNames HarassmentThreatening = new("harassment/threatening");

    public static readonly OpenAIModerationCategoryNames Hate = new("hate");

    public static readonly OpenAIModerationCategoryNames HateThreatening = new("hate/threatening");

    public static readonly OpenAIModerationCategoryNames Illicit = new("illicit");

    public static readonly OpenAIModerationCategoryNames IllicitViolent = new("illicit/violent");

    public static readonly OpenAIModerationCategoryNames SelfHarm = new("self-harm");

    public static readonly OpenAIModerationCategoryNames SelfHarmIntent = new("self-harm/intent");

    public static readonly OpenAIModerationCategoryNames SelfHarmInstructions = new("self-harm/instructions");

    public static readonly OpenAIModerationCategoryNames Sexual = new("sexual");

    public static readonly OpenAIModerationCategoryNames SexualMinors = new("sexual/minors");

    public static readonly OpenAIModerationCategoryNames Violence = new("violence");

    public static readonly OpenAIModerationCategoryNames ViolenceGraphic = new("violence/graphic");
}
