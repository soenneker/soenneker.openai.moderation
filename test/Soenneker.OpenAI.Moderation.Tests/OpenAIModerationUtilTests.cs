using System.Collections.Generic;
using System.IO;
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
using Soenneker.OpenAI.Moderation.Models;
using Soenneker.OpenAI.Moderation.Options;
using Soenneker.OpenAI.OpenApiClient;
using Soenneker.OpenAI.OpenApiClient.Models;
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

        var clientUtil = new Mock<IOpenAIModerationOpenApiClientUtil>();
        clientUtil.Setup(util => util.Get(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new OpenAIOpenApiClient(requestAdapter.Object));

        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            [OpenAIModerationDefaults.EnabledConfigurationKey] = "true",
            [OpenAIModerationDefaults.ModelConfigurationKey] = OpenAIModerationDefaults.Model
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
    public async ValueTask ModerateMultimodal_WithImageAndText_SerializesExpectedInput()
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
                requestInformation = request)
            .ReturnsAsync(new CreateModerationResponse());

        var clientUtil = new Mock<IOpenAIModerationOpenApiClientUtil>();
        clientUtil.Setup(util => util.Get(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new OpenAIOpenApiClient(requestAdapter.Object));

        var util = new OpenAIModerationUtil(clientUtil.Object, new ConfigurationBuilder().Build());

        await util.ModerateMultimodal(
        [
            OpenAIModerationInput.FromText("context"),
            OpenAIModerationInput.FromImageUrl("https://example.com/image.png"),
            OpenAIModerationInput.FromBase64Image("YWJj", "image/png")
        ], new OpenAIModerationOptions(), CancellationToken.None);

        requestInformation.Should().NotBeNull();
        requestInformation!.Content.Should().NotBeNull();
        requestInformation.Content!.Position = 0;

        using var reader = new StreamReader(requestInformation.Content, leaveOpen: true);
        string json = await reader.ReadToEndAsync(CancellationToken.None);

        json.Should().Contain("\"type\":\"text\"");
        json.Should().Contain("\"text\":\"context\"");
        json.Should().Contain("\"type\":\"image_url\"");
        json.Should().Contain("\"image_url\":{\"url\":\"https://example.com/image.png\"}");
        json.Should().Contain("\"image_url\":{\"url\":\"data:image/png;base64,YWJj\"}");
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
