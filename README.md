[![](https://img.shields.io/nuget/v/soenneker.openai.moderation.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.openai.moderation/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.openai.moderation/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.openai.moderation/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.openai.moderation.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.openai.moderation/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.OpenAI.Moderation
### A .NET OpenAI content moderation utility using their OpenAPI client

## Installation

```
dotnet add package Soenneker.OpenAI.Moderation
```

## Usage

```csharp
using Soenneker.OpenAI.Moderation.Abstract;
using Soenneker.OpenAI.Moderation.Extensions;
using Soenneker.OpenAI.Moderation.Registrars;
using Soenneker.OpenAI.OpenApiClient.Models;

services.AddOpenAIModerationUtilAsScoped();

CreateModerationResponse? response = await moderationUtil.Moderate("content to check", cancellationToken);

if (response.IsFlagged())
{
    string? category = response.GetFirstFlaggedCategoryValue();
}
```

Configuration defaults:

```json
{
  "OpenAI": {
    "ApiKey": "...",
    "Moderation": {
      "Enabled": true,
      "Model": "omni-moderation-latest",
      "SkipWhenApiKeyMissing": true
    }
  }
}
```
