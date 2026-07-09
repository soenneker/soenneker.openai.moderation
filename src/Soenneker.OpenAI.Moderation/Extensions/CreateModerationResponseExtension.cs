using System.Collections.Generic;
using System.Linq;
using Soenneker.OpenAI.Moderation.Constants;
using Soenneker.OpenAI.OpenApiClient.Models;
using OpenAIModerationCategoryNames = Soenneker.OpenAI.Moderation.Enums.OpenAIModerationCategoryNames;

namespace Soenneker.OpenAI.Moderation.Extensions;

/// <summary>
/// Convenience helpers for OpenAI moderation response models.
/// </summary>
public static class CreateModerationResponseExtension
{
    /// <summary>
    /// Determines whether any moderation result in the response is flagged.
    /// </summary>
    /// <param name="response">The moderation response to inspect.</param>
    /// <returns><see langword="true"/> when any result is flagged; otherwise <see langword="false"/>.</returns>
    public static bool IsFlagged(this CreateModerationResponse? response)
    {
        return response?.Results?.Any(result => result.Flagged == true) == true;
    }

    /// <summary>
    /// Gets the first flagged moderation result from a response.
    /// </summary>
    /// <param name="response">The moderation response to inspect.</param>
    /// <returns>The first flagged result, or <see langword="null"/> when no result is flagged.</returns>
    public static CreateModerationResponseResultsItem? GetFirstFlaggedResult(this CreateModerationResponse? response)
    {
        return response?.Results?.FirstOrDefault(result => result.Flagged == true);
    }

    /// <summary>
    /// Gets the first flagged category from a moderation response.
    /// </summary>
    /// <param name="response">The moderation response to inspect.</param>
    /// <returns>The first flagged category, or <see langword="null"/> when no category is flagged.</returns>
    public static OpenAIModerationCategoryNames? GetFirstFlaggedCategory(this CreateModerationResponse? response)
    {
        return response.GetFirstFlaggedResult()?.GetFirstFlaggedCategory();
    }

    /// <summary>
    /// Gets the string value of the first flagged category from a moderation response.
    /// </summary>
    /// <param name="response">The moderation response to inspect.</param>
    /// <returns>The first flagged category value, or <see langword="null"/> when no category is flagged.</returns>
    public static string? GetFirstFlaggedCategoryValue(this CreateModerationResponse? response)
    {
        return response.GetFirstFlaggedCategory()?.Value;
    }

    /// <summary>
    /// Gets the first flagged category from a moderation result.
    /// </summary>
    /// <param name="result">The moderation result to inspect.</param>
    /// <returns>The first flagged category, or <see langword="null"/> when no category is flagged.</returns>
    public static OpenAIModerationCategoryNames? GetFirstFlaggedCategory(this CreateModerationResponseResultsItem? result)
    {
        return result.GetFlaggedCategories().FirstOrDefault();
    }

    /// <summary>
    /// Gets the string value of the first flagged category from a moderation result.
    /// </summary>
    /// <param name="result">The moderation result to inspect.</param>
    /// <returns>The first flagged category value, or <see langword="null"/> when no category is flagged.</returns>
    public static string? GetFirstFlaggedCategoryValue(this CreateModerationResponseResultsItem? result)
    {
        return result.GetFirstFlaggedCategory()?.Value;
    }

    /// <summary>
    /// Gets all flagged categories across every result in a moderation response.
    /// </summary>
    /// <param name="response">The moderation response to inspect.</param>
    /// <returns>A list of distinct flagged categories.</returns>
    public static IReadOnlyList<OpenAIModerationCategoryNames> GetFlaggedCategories(this CreateModerationResponse? response)
    {
        if (response?.Results is not {Count: > 0} results)
            return [];

        return results.SelectMany(result => result.GetFlaggedCategories()).Distinct().ToList();
    }

    /// <summary>
    /// Gets all flagged category string values across every result in a moderation response.
    /// </summary>
    /// <param name="response">The moderation response to inspect.</param>
    /// <returns>A list of distinct flagged category values.</returns>
    public static IReadOnlyList<string> GetFlaggedCategoryValues(this CreateModerationResponse? response)
    {
        return response.GetFlaggedCategories().Select(category => category.Value).ToList();
    }

    /// <summary>
    /// Gets all flagged categories from a moderation result.
    /// </summary>
    /// <param name="result">The moderation result to inspect.</param>
    /// <returns>A list of flagged categories.</returns>
    public static IReadOnlyList<OpenAIModerationCategoryNames> GetFlaggedCategories(this CreateModerationResponseResultsItem? result)
    {
        CreateModerationResponseResultsItemCategories? categories = result?.Categories;

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

        if (categories.Illicit?.Value == true)
            flaggedCategories.Add(OpenAIModerationCategoryNames.Illicit);

        if (categories.IllicitViolent?.Value == true)
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

    /// <summary>
    /// Gets all flagged category string values from a moderation result.
    /// </summary>
    /// <param name="result">The moderation result to inspect.</param>
    /// <returns>A list of flagged category values.</returns>
    public static IReadOnlyList<string> GetFlaggedCategoryValues(this CreateModerationResponseResultsItem? result)
    {
        return result.GetFlaggedCategories().Select(category => category.Value).ToList();
    }

}
