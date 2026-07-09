using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.OpenAI.Moderation.Abstract;
using Soenneker.OpenAI.Moderation.Constants;
using Soenneker.OpenAI.Moderation.Options;
using Soenneker.OpenAI.OpenApiClient;
using Soenneker.OpenAI.OpenApiClient.Models;
using Soenneker.OpenAI.OpenApiClientUtil.Abstract;

namespace Soenneker.OpenAI.Moderation;

/// <inheritdoc cref="IOpenAIModerationUtil"/>
public sealed class OpenAIModerationUtil : IOpenAIModerationUtil
{
    private readonly IOpenAIOpenApiClientUtil _clientUtil;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIModerationUtil"/> class.
    /// </summary>
    /// <param name="clientUtil">The OpenAI OpenAPI client provider.</param>
    /// <param name="configuration">The application configuration used for default moderation options.</param>
    public OpenAIModerationUtil(IOpenAIOpenApiClientUtil clientUtil, IConfiguration configuration)
    {
        _clientUtil = clientUtil;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public ValueTask<CreateModerationResponse?> Moderate(string input, CancellationToken cancellationToken = default)
    {
        return Moderate(input, OpenAIModerationOptions.FromConfiguration(_configuration), cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<CreateModerationResponse?> Moderate(string input, OpenAIModerationOptions options,
        CancellationToken cancellationToken = default)
    {
        return Moderate([input], options, cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs,
        CancellationToken cancellationToken = default)
    {
        return Moderate(inputs, OpenAIModerationOptions.FromConfiguration(_configuration), cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<CreateModerationResponse?> Moderate(IReadOnlyList<string> inputs,
        OpenAIModerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(options);

        if (!options.Enabled)
            return null;

        if (inputs.Count == 0 || inputs.All(string.IsNullOrWhiteSpace))
            return null;

        OpenAIOpenApiClient client = await _clientUtil.Get(cancellationToken);

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
            String = inputs.ToList()
        };
    }
}
