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

    public OpenAIModerationUtil(IOpenAIOpenApiClientUtil clientUtil, IConfiguration configuration)
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

        OpenAIOpenApiClient client = await _clientUtil.Get(cancellationToken);

        return await client.Moderations.PostAsync(new CreateModerationRequest
        {
            Input = ToInput(inputs),
            Model = new CreateModerationRequest.CreateModerationRequest_model
            {
                String = string.IsNullOrWhiteSpace(options.Model) ? OpenAIModerationDefaults.Model : options.Model
            }
        }, cancellationToken: cancellationToken);
    }

    private static CreateModerationRequest.CreateModerationRequest_input ToInput(IReadOnlyList<string> inputs)
    {
        if (inputs.Count == 1)
        {
            return new CreateModerationRequest.CreateModerationRequest_input
            {
                CreateModerationRequestInputString = inputs[0]
            };
        }

        return new CreateModerationRequest.CreateModerationRequest_input
        {
            String = inputs.ToList()
        };
    }
}