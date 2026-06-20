using System;
using System.Collections.Generic;
using System.Linq;
using Soenneker.OpenAI.Moderation.Constants;
using Soenneker.OpenAI.OpenApiClient.Models;

namespace Soenneker.OpenAI.Moderation.Extensions;

/// <summary>
/// Convenience helpers for OpenAI moderation response models.
/// </summary>
public static class CreateModerationResponseExtension
{
    public static bool IsFlagged(this CreateModerationResponse? response)
    {
        return response?.Results?.Any(result => result.Flagged == true) == true;
    }

    public static CreateModerationResponse_results? GetFirstFlaggedResult(this CreateModerationResponse? response)
    {
        return response?.Results?.FirstOrDefault(result => result.Flagged == true);
    }

    public static OpenAIModerationCategoryNames? GetFirstFlaggedCategory(this CreateModerationResponse? response)
    {
        return response.GetFirstFlaggedResult()?.GetFirstFlaggedCategory();
    }

    public static string? GetFirstFlaggedCategoryValue(this CreateModerationResponse? response)
    {
        return response.GetFirstFlaggedCategory()?.Value;
    }

    public static OpenAIModerationCategoryNames? GetFirstFlaggedCategory(this CreateModerationResponse_results? result)
    {
        return result.GetFlaggedCategories().FirstOrDefault();
    }

    public static string? GetFirstFlaggedCategoryValue(this CreateModerationResponse_results? result)
    {
        return result.GetFirstFlaggedCategory()?.Value;
    }

    public static IReadOnlyList<OpenAIModerationCategoryNames> GetFlaggedCategories(this CreateModerationResponse? response)
    {
        if (response?.Results is not {Count: > 0} results)
            return [];

        return results.SelectMany(result => result.GetFlaggedCategories()).Distinct().ToList();
    }

    public static IReadOnlyList<string> GetFlaggedCategoryValues(this CreateModerationResponse? response)
    {
        return response.GetFlaggedCategories().Select(category => category.Value).ToList();
    }

    public static IReadOnlyList<OpenAIModerationCategoryNames> GetFlaggedCategories(this CreateModerationResponse_results? result)
    {
        CreateModerationResponse_results_categories? categories = result?.Categories;

        if (categories == null)
            return [];

        var flaggedCategories = new List<OpenAIModerationCategoryNames>();

        if (categories.Harassment == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.Harassment);

        if (categories.HarassmentThreatening == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.HarassmentThreatening);

        if (categories.Hate == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.Hate);

        if (categories.HateThreatening == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.HateThreatening);

        if (IsTrue(categories.Illicit?.Value))
            flaggedCategories.Add(OpenAIModerationCategoryNames.Illicit);

        if (IsTrue(categories.IllicitViolent?.Value))
            flaggedCategories.Add(OpenAIModerationCategoryNames.IllicitViolent);

        if (categories.SelfHarm == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.SelfHarm);

        if (categories.SelfHarmIntent == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.SelfHarmIntent);

        if (categories.SelfHarmInstructions == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.SelfHarmInstructions);

        if (categories.Sexual == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.Sexual);

        if (categories.SexualMinors == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.SexualMinors);

        if (categories.Violence == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.Violence);

        if (categories.ViolenceGraphic == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.ViolenceGraphic);

        return flaggedCategories;
    }

    public static IReadOnlyList<string> GetFlaggedCategoryValues(this CreateModerationResponse_results? result)
    {
        return result.GetFlaggedCategories().Select(category => category.Value).ToList();
    }

    private static bool IsTrue(string? value)
    {
        return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }
}
