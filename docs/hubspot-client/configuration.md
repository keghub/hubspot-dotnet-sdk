As for most libraries targeting the newest .NET Core components, this library exposes extension methods that help attach the configuration to the service collection.

```csharp
var services = new ServiceCollection();

services.AddHubSpotClient(client => 
{
    // ... here goes the configuration part.
});
```

The `client` variable passed to the configuration delegate is of type `IHubSpotClientConfigurator` and can be used to
- customize how the `HttpClient` is crated by the internal `IHttpClientFactory`
- register internal services

Additional extension methods to the `IHubSpotClientConfigurator` type are available and can be added.

## Configure the base address

By default, the client is configured to target the HubSpot REST API endpoint, https://api.hubapi.com.

For development and/or troubleshooting reasons, it is possible to override this setting by using the `SetBaseAddress` method.

```csharp
services.AddHubSpotClient(client => 
{
    client.SetBaseAddress(new Uri("https://localhost.temp"));
});
```

## Configure the serialization settings

By default, the client is configured to serialize the payloads according to HubSpot REST API's guidelines.

In case of need, it's possible to customize the default setup by using the `ConfigureSerialization` method.

```csharp
services.AddHubSpotClient(client => 
{
    client.ConfigureSerialization(settings => 
    {
        settings.Formatting = Newtonsoft.Json.Formatting.Indented;
    });
});
```

## Configure the HTTP client

In case of need, it's possible to customize the `HttpClient` used to perform the HTTP requests to the HubSpot API.

For this, it's possible to use the `ConfigureHttpClient` method.

```csharp
services.AddHubSpotClient(client => 
{
    client.ConfigureHttpClient(http => 
    {
        http.DefaultRequestHeaders.Add("X-CustomHeader", "my value");
    });
});
```

## Configure the HTTP client factory

The `HttpClient` factory API allows the creation of complex chains of handlers that can be used to enrich and inspect the outgoing HTTP request.

Use cases range from adding support for tracers like [AWS X-Ray](https://aws.amazon.com/xray/) or a transient-fault-handling library like [Polly](https://github.com/App-vNext/Polly).

### Sample: Configuring AWS X-Ray tracing.

The snippet below shows how to instruct the HTTP client factory to decorate clients with the AWS X-Ray tracing handler.

```csharp
services.AddHubSpotClient(client => 
{
    client.ConfigureHttpClientBuilder(builder => 
    {
        builder.AddHttpMessageHandler<HttpClientXRayTracingHandler>();
    });
});
```

### Sample: Configuring a Polly policy

The snippet below shows how to instruct the HTTP client factory to decorate clients with a retry policy created with Polly.

```csharp
services.AddHubSpotClient(client => 
{
    client.ConfigureHttpClientBuilder(builder =>
    {
        builder.AddPolicyHandler(GetRetryPolicy());
    });
});

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
```

## Authentication

HubSpot's APIs allow two means of authentication: OAuth and API keys.

While most endpoints support both methods (unless the documentation for a specific endpoint says otherwise), OAuth is generally recommended.

This library supports both types.

### OAuth authentication

OAuth is used to require an authorization token to be used to sign successive requests.

The whole flow is automatically handled via a message handler added to the HttpClient pipeline.

The snippet below shows how to register the OAuth message handler.

```csharp
services.AddHubSpotClient(client => 
{
    client.UseOAuthAuthentication(configuration.GetSection("HubSpot"));
});
```

In the snippet, we are assuming that the following fields are available through the Microsoft Configuration subsystem, under the parent key `HubSpot`:
- `HubSpot:ClientId`
- `HubSpot:SecretKey`
- `HubSpot:RedirectUri`
- `HubSpot:RefreshToken`

### API key authentication

API keys are good for rapid prototyping or integrations designed for single-account use. HubSpot API requires the API key to be added as a query string parameter whose key is `hapikey`.

This library takes care of properly attaching the API key to the outgoing requests via a message handler added to the HttpClient pipeline.

The snippet below shows how to register the API key message handler.

```csharp
services.AddHubSpotClient(client => 
{
    client.UseApiKeyAuthentication(configuration.GetSection("HubSpot"));
});
```

In the snippet, we are assuming that the API key is available through the Microsoft Configuration subsystem, under the parent key `HubSpot` with the name `ApiKey`: `HubSpot:ApiKey`.

## Daisy chaining

The configuration API supports daisy chaining of methods.

```csharp
services.AddHubSpotClient(client => client
            .SetBaseAddress(new Uri("https://localhost.temp"))
            .UseOAuthAuthentication(configuration.GetSection("HubSpot"))
            .ConfigureSerialization(settings => settings.Formatting = Newtonsoft.Json.Formatting.Indented)
            .ConfigureHttpClient(http => http.DefaultRequestHeaders.Add("X-CustomHeader", "my value"))
            .ConfigureHttpClientBuilder(builder => builder
                .AddHttpMessageHandler<HttpClientXRayTracingHandler>()
                .AddPolicyHandler(GetRetryPolicy())));
```
