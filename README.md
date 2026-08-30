[![](https://img.shields.io/nuget/v/soenneker.openai.moderation.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.openai.moderation/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.openai.moderation/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.openai.moderation/actions/workflows/publish-package.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.openai.moderation/codeql.yml?label=codeql&style=for-the-badge)](https://github.com/soenneker/soenneker.openai.moderation/actions/workflows/codeql.yml)
[![](https://img.shields.io/nuget/dt/soenneker.openai.moderation.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.openai.moderation/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.OpenAI.Moderation
Moderate text with OpenAI and inspect flagged categories through typed response helpers.

## Installation

```bash
dotnet add package Soenneker.OpenAI.Moderation
```

## Registration and configuration

Register the scoped utility during application startup:

```csharp
using Soenneker.OpenAI.Moderation.Registrars;

services.AddOpenAIModerationUtilAsScoped();
```

The scoped registration keeps the utility scoped while reusing the singleton OpenAI client. A singleton utility registration is also available through `AddOpenAIModerationUtilAsSingleton()`.

Provide the API key through configuration. `Enabled` defaults to `true`, and `Model` defaults to `omni-moderation-latest` when omitted:

```json
{
  "OpenAI": {
    "ApiKey": "...",
    "Moderation": {
      "Enabled": true,
      "Model": "omni-moderation-latest"
    }
  }
}
```

Keep the API key in a secret store or environment variable rather than committed configuration.

## Moderate text

Inject `IOpenAIModerationUtil`, then inspect the returned OpenAI response directly or use the supplied helpers:

```csharp
using Soenneker.OpenAI.Moderation.Abstract;
using Soenneker.OpenAI.Moderation.Extensions;
using Soenneker.OpenAI.OpenApiClient.Models;

CreateModerationResponse? response = await moderationUtil.Moderate("content to check", cancellationToken);

if (response.IsFlagged())
{
    IReadOnlyList<string> categories = response.GetFlaggedCategoryValues();
}
```

`Moderate` returns `null` when moderation is disabled, the input is empty, or every item in a batch is blank. API and transport failures are allowed to propagate to the caller.

Moderate several inputs in one request:

```csharp
CreateModerationResponse? response = await moderationUtil.Moderate(
    ["first message", "second message"],
    cancellationToken);
```

Use per-call options when behavior should differ from configuration:

```csharp
var options = new OpenAIModerationOptions
{
    Enabled = true,
    Model = "omni-moderation-latest"
};

CreateModerationResponse? response = await moderationUtil.Moderate(
    "content to check",
    options,
    cancellationToken);
```
