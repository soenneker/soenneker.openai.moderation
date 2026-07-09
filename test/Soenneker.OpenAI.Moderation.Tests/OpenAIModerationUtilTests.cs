using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;
using Moq;
using Soenneker.OpenAI.Moderation.Abstract;
using Soenneker.OpenAI.Moderation.Constants;
using Soenneker.OpenAI.Moderation.Extensions;
using Soenneker.OpenAI.Moderation.Options;
using Soenneker.OpenAI.OpenApiClient;
using Soenneker.OpenAI.OpenApiClient.Models;
using Soenneker.OpenAI.OpenApiClientUtil.Abstract;
using Soenneker.Tests.Attributes.Local;
using Soenneker.Tests.HostedUnit;
using OpenAIModerationCategoryNames = Soenneker.OpenAI.Moderation.Enums.OpenAIModerationCategoryNames;

namespace Soenneker.OpenAI.Moderation.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class OpenAIModerationUtilTests : HostedUnitTest
{
    private readonly IOpenAIModerationUtil _util;

    public OpenAIModerationUtilTests(Host host) : base(host)
    {
        _util = Resolve<IOpenAIModerationUtil>(true);
    }

    [Test]
    public void Default()
    {
    }

    [Test]
    public async ValueTask Moderate_WhenOpenAIFlagsInput_ReturnsFlaggedResult()
    {
        RequestInformation? requestInformation = null;
        var requestAdapter = new Mock<IRequestAdapter>();
        requestAdapter.SetupProperty(adapter => adapter.BaseUrl, "https://api.openai.com/v1");
        requestAdapter.SetupGet(adapter => adapter.SerializationWriterFactory)
                      .Returns(new JsonSerializationWriterFactory());
        requestAdapter
            .Setup(adapter => adapter.SendAsync(It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<CreateModerationResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(), It.IsAny<CancellationToken>()))
            .Callback<RequestInformation, ParsableFactory<CreateModerationResponse>,
                Dictionary<string, ParsableFactory<IParsable>>, CancellationToken>((request, _, _, _) =>
                requestInformation = request).ReturnsAsync(new CreateModerationResponse
            {
                Results =
                [
                    new CreateModerationResponseResultsItem
                    {
                        Flagged = true,
                        Categories = new CreateModerationResponseResultsItemCategories
                        {
                            Violence = true
                        },
                        CategoryScores = new CreateModerationResponseResultsItemCategoryScores
                        {
                            Violence = .98
                        }
                    }
                ]
            });

        var clientUtil = new Mock<IOpenAIOpenApiClientUtil>();
        clientUtil.Setup(util => util.Get(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new OpenAIOpenApiClient(requestAdapter.Object));

        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            [OpenAIModerationDefaults.ApiKeyConfigurationKey] = "test-key"
        }).Build();

        var util = new OpenAIModerationUtil(clientUtil.Object, configuration);

        CreateModerationResponse? result = await util.Moderate("violent test message", new OpenAIModerationOptions(),
            CancellationToken.None);

        result.IsFlagged().Should().BeTrue();
        result.GetFirstFlaggedCategory().Should().Be(OpenAIModerationCategoryNames.Violence);
        result.GetFirstFlaggedCategoryValue().Should().Be("violence");
        result?.Results?[0].CategoryScores?.Violence.Should().Be(.98);
        requestInformation.Should().NotBeNull();
        requestInformation!.HttpMethod.Should().Be(Method.POST);
        requestInformation.UrlTemplate.Should().Contain("/moderations");
    }

    [Test]
    public async ValueTask Moderate_WhenDisabled_Skips()
    {
        CreateModerationResponse? result = await _util.Moderate("test", new OpenAIModerationOptions
        {
            Enabled = false
        }, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    [LocalOnly]
    public async ValueTask Test(CancellationToken cancellationToken)
    {
        CreateModerationResponse? result = await _util.Moderate("test", new OpenAIModerationOptions
        {
            Enabled = true,
        }, cancellationToken);

        result.Should().NotBeNull();
    }
}
