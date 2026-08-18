using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.OpenAI.Moderation.Abstract;
using Soenneker.OpenAI.Moderation.HttpClients.Abstract;
using Soenneker.OpenAI.OpenApiClient;

namespace Soenneker.OpenAI.Moderation;

/// <inheritdoc cref="IOpenAIModerationOpenApiClientUtil"/>
public sealed class OpenAIModerationOpenApiClientUtil : IOpenAIModerationOpenApiClientUtil
{
    private readonly IOpenAIModerationHttpClient _httpClient;

    public OpenAIModerationOpenApiClientUtil(IOpenAIModerationHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<OpenAIOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        HttpClient httpClient = await _httpClient.Get(cancellationToken);
        var requestAdapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: httpClient);
        return new OpenAIOpenApiClient(requestAdapter);
    }
}
