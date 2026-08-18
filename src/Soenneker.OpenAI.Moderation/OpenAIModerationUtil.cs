using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Serialization;
using Soenneker.OpenAI.Moderation.Abstract;
using Soenneker.OpenAI.Moderation.Constants;
using Soenneker.OpenAI.Moderation.Models;
using Soenneker.OpenAI.Moderation.Options;
using Soenneker.OpenAI.OpenApiClient;
using Soenneker.OpenAI.OpenApiClient.Models;

namespace Soenneker.OpenAI.Moderation;

/// <inheritdoc cref="IOpenAIModerationUtil"/>
public sealed class OpenAIModerationUtil : IOpenAIModerationUtil
{
    private readonly IOpenAIModerationOpenApiClientUtil _clientUtil;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIModerationUtil"/> class.
    /// </summary>
    /// <param name="clientUtil">The OpenAI OpenAPI client provider.</param>
    /// <param name="configuration">The application configuration used for default moderation options.</param>
    public OpenAIModerationUtil(IOpenAIModerationOpenApiClientUtil clientUtil, IConfiguration configuration)
    {
        _clientUtil = clientUtil;
        _configuration = configuration;
    }

    public ValueTask<CreateModerationResponse?> Moderate(string input, CancellationToken cancellationToken = default)
    {
        return Moderate(input, OpenAIModerationOptions.FromConfiguration(_configuration), cancellationToken);
    }

    public ValueTask<CreateModerationResponse?> Moderate(string input, OpenAIModerationOptions options,
        CancellationToken cancellationToken = default)
    {
        return Moderate([input], options, cancellationToken);
    }

    public ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs,
        CancellationToken cancellationToken = default)
    {
        return Moderate(inputs, OpenAIModerationOptions.FromConfiguration(_configuration), cancellationToken);
    }

    public async ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs,
        OpenAIModerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(options);

        if (!options.Enabled)
            return null;

        if (inputs.Count == 0 || inputs.All(string.IsNullOrWhiteSpace))
            return null;

        OpenAIOpenApiClient client = await GetClient(cancellationToken);

        return await client.Moderations.PostAsync(new CreateModerationRequest
        {
            Input = ToInput(inputs),
            Model = new CreateModerationRequestModel
            {
                CreateModerationRequestModelBranch1 = new CreateModerationRequestModelBranch1
                {
                    Value = string.IsNullOrWhiteSpace(options.Model) ? OpenAIModerationDefaults.Model : options.Model
                }
            }
        }, cancellationToken: cancellationToken);
    }

    public ValueTask<CreateModerationResponse?> ModerateImage(string imageUrl, CancellationToken cancellationToken = default)
    {
        return ModerateImage(imageUrl, OpenAIModerationOptions.FromConfiguration(_configuration), cancellationToken);
    }

    public ValueTask<CreateModerationResponse?> ModerateImage(string imageUrl, OpenAIModerationOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (!options.Enabled)
            return ValueTask.FromResult<CreateModerationResponse?>(null);

        return ModerateMultimodal([OpenAIModerationInput.FromImageUrl(imageUrl)], options, cancellationToken);
    }

    public ValueTask<CreateModerationResponse?> ModerateBase64Image(string base64, string mediaType,
        CancellationToken cancellationToken = default)
    {
        return ModerateBase64Image(base64, mediaType, OpenAIModerationOptions.FromConfiguration(_configuration), cancellationToken);
    }

    public ValueTask<CreateModerationResponse?> ModerateBase64Image(string base64, string mediaType, OpenAIModerationOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (!options.Enabled)
            return ValueTask.FromResult<CreateModerationResponse?>(null);

        return ModerateMultimodal([OpenAIModerationInput.FromBase64Image(base64, mediaType)], options, cancellationToken);
    }

    public ValueTask<CreateModerationResponse?> ModerateMultimodal(IReadOnlyList<OpenAIModerationInput> inputs,
        CancellationToken cancellationToken = default)
    {
        return ModerateMultimodal(inputs, OpenAIModerationOptions.FromConfiguration(_configuration), cancellationToken);
    }

    public async ValueTask<CreateModerationResponse?> ModerateMultimodal(IReadOnlyList<OpenAIModerationInput> inputs,
        OpenAIModerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(options);

        if (!options.Enabled || inputs.Count == 0)
            return null;

        OpenAIOpenApiClient client = await GetClient(cancellationToken);

        return await client.Moderations.PostAsync(new CreateModerationRequest
        {
            Input = ToInput(inputs),
            Model = new CreateModerationRequestModel
            {
                CreateModerationRequestModelBranch1 = new CreateModerationRequestModelBranch1
                {
                    Value = string.IsNullOrWhiteSpace(options.Model) ? OpenAIModerationDefaults.Model : options.Model
                }
            }
        }, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Converts text inputs to the generated OpenAI moderation request input wrapper.
    /// </summary>
    /// <param name="inputs">The text inputs to convert.</param>
    /// <returns>The generated request input wrapper.</returns>
    private static CreateModerationRequestInput ToInput(IReadOnlyList<string> inputs)
    {
        if (inputs.Count == 1)
        {
            return new CreateModerationRequestInput
            {
                CreateModerationRequestInputString = inputs[0]
            };
        }

        return new CreateModerationRequestInput
        {
            String = [.. inputs]
        };
    }

    private ValueTask<OpenAIOpenApiClient> GetClient(CancellationToken cancellationToken)
    {
        return _clientUtil.Get(cancellationToken);
    }

    private static CreateModerationRequestInput ToInput(IReadOnlyList<OpenAIModerationInput> inputs)
    {
        var items = new List<CreateModerationRequestInputOneOf3Item>(inputs.Count);

        foreach (OpenAIModerationInput input in inputs)
        {
            ArgumentNullException.ThrowIfNull(input);

            var item = new CreateModerationRequestInputOneOf3Item();

            if (input.Type == OpenAIModerationInputType.Text)
            {
                item.AdditionalData["type"] = "text";
                item.AdditionalData["text"] = input.Value;
            }
            else
            {
                item.AdditionalData["type"] = "image_url";
                item.AdditionalData["image_url"] = new ModerationImageUrl(input.Value);
            }

            items.Add(item);
        }

        return new CreateModerationRequestInput
        {
            CreateModerationRequestInputOneOf3Item = items
        };
    }

    private sealed class ModerationImageUrl(string url) : IParsable
    {
        public IDictionary<string, Action<IParseNode>> GetFieldDeserializers() => new Dictionary<string, Action<IParseNode>>();

        public void Serialize(ISerializationWriter writer)
        {
            writer.WriteStringValue("url", url);
        }
    }

}
