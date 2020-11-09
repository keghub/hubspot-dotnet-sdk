Like `EMG.HubSpot.Client`, this library also adds extension methods to `IServiceCollection` to quickly register all the needed components.

```csharp
var services = new ServiceCollection();

services.AddHubSpot(hs => 
{
    // ... here goes the configuration part.
});
```

The `hs` variable passed to the configuration delegate is an instance of type `IHubSpotConfigurator` and expose methods that can be used to:
- configure the underlying `IHubSpotClient`
- register internal services.

Additional extension methods to the `IHubSpotConfigurator` type are available and can be added.

## Configure the HubSpot client

While being a higher-layer library, this library allows full access to the customization of the HubSpot client as described [earlier](../hubspot-client/configuration.md).

This can be achieved using the `AddClientConfiguration` method.

In the snippet below, we're registering all the customizations we added earlier.

```csharp
services.AddHubSpot(hs => 
{
    hs.AddClientConfiguration(client => client
        .SetBaseAddress(new Uri("https://localhost.temp"))
        .UseOAuthAuthentication(configuration.GetSection("HubSpot"))
        .ConfigureSerialization(settings => settings.Formatting = Newtonsoft.Json.Formatting.Indented)
        .ConfigureHttpClient(http => http.DefaultRequestHeaders.Add("X-CustomHeader", "my value"))
        .ConfigureHttpClientBuilder(builder => builder
            .AddHttpMessageHandler<HttpClientXRayTracingHandler>()
            .AddPolicyHandler(GetRetryPolicy())));
});
```

## Authentication

Since HubSpot always require any of the two supported authentication methods, convenience methods are offered to set up the authentication method without the need of using `AddClientConfiguration`.

```csharp
services.AddHubSpot(hs => 
{
    hs.UseOAuthAuthentication(configuration.GetSection("HubSpot"));
});

services.AddHubSpot(hs => 
{
    hs.UseApiKeyAuthentication(configuration.GetSection("HubSpot"));
});
```

The same assumptions done [earlier](../hubspot-client/configuration.md#Authentication) apply.