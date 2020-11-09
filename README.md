# EMG .NET SDK for HubSpot

This repository contains an SDK for .NET applications needing to integrate with [HubSpot](https://www.hubspot.com).

This SDK is composed of two packages:

- `EMG.HubSpot.Client`,
- `EMG.HubSpot`

## EMG.HubSpot.Client

`EMG.HubSpot.Client` contains a library that maps 1-to-1 the [HubSpot REST API](https://developers.hubspot.com/docs/api/overview).

This library uses `Kralizek.Extensions.Http` to preform the needed HTTP communication and supports both [OAuth](https://developers.hubspot.com/docs/api/working-with-oauth) and API Key authentication flows.

### Configuration

Here is an example of an application configured to query HubSpot via an API key using this library.

```csharp
var configuration = new ConfigurationBuilder()
                            .AddEnvironmentVariables()
                            .Build();

var services = new ServiceCollection();

services.AddHubSpotClient(client => client
        .UseApiKeyAuthentication(configuration.GetSection("HubSpot")));

services.AddLogging(b => b.AddConsole());

var serviceProvider = services.BuildServiceProvider();

var client = serviceProvider.GetRequiredService<IHubSpotClient>();

DoStuffWithHubSpot(client);
```

In the sample above, we are assuming an environment variable named `HubSpot__ApiKey` contains the HubSpot API key to be used to sign the requests.

## EMG.HubSpot

`EMG.HubSpot` contains a high-level library that uses `EMG.HubSpot.Client` to retrieve information and allows the developer to work with POCO types.

### Connectors

This library is based on _connectors_.

A connector exposes a friendlier API that can be used to interact with their specific type, regardles of the underlying implementation.

For example, the `IHubSpotCompanyConnector` supports the possibility to retrieve all the companies matching a certain criteria ignoring the underlying paging system.

```csharp
var connector = serviceProvider.GetRequiredService<IHubSpotCompanyConnector>();

var companies = await connector.FindAsync(FilterCompanies.ByDomain("educations.com"));

foreach (var company in companies)
{
    DoSomethingWithCompany(company);
}
```

Currently there are three connectors:

- `IHubSpotCompanyConnector` can be used to interact with companies
- `IHubSpotContactConnector` can be used to interact with contacts
- `IHubSpotDealConnector` can be used to interact with deals

Each connector needs to be registered independently.

```csharp
services.AddHubSpot(hs => hs
        .UseOAuthAuthentication(configuration.GetSection("HubSpot"))
        .UseCompanyConnector()
        .UseContactConnector()
        .UseDealConnector());
```

### Customized entities

The entities exposed by connectors can be expanded via subclassing to add custom properties

For example, in the snippet below we are creating a `CustomDeal` type so that we can add our own property.

```csharp
public class CustomDeal : HubSpot.Deals.Deal
{
    [CustomProperty("internal_id")]
    public long InternalId { get; set; }
}
```

When using the connector, the library will take care of serializing the value of the `InternalId` property into a custom property named `internal_id`. Or, viceversa, it will request the field and deserialize it into the property of the object.

```csharp
var connector = serviceProvider.GetRequiredService<IHubSpotDealConnector>();

var deal = await connector.GetByIdAsync<CustomDeal>(123);

Assert.That(deal.InternalId, Is.Not.Null);
```

### Type converters

Another feature offered by connectors is the ability to use custom serializers to persist complex objects into HubSpot custom properties.

In case of need, simply create a class implementing the interface `ITypeConverter` and register it at startup.

Here is an example on how to register a custom converter.)

```csharp
services.AddHubSpot(hs => hs
        .RegisterConverter(new MyCustomTypeConverter(), typeof(MyCustomType))
        .UseCompanyConnector()
        .UseContactConnector());
```

### Configuration

Here is an example of an application configured to query HubSpot via OAuth using this library.

```csharp
var configuration = new ConfigurationBuilder()
                            .AddEnvironmentVariables()
                            .Build();

var services = new ServiceCollection();

services.AddHubSpot(hs => hs
        .UseOAuthAuthentication(configuration.GetSection("HubSpot"))
        .UseCompanyConnector()
        .UseContactConnector());

services.AddLogging(b => b.AddConsole());

var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

var companyConnector = serviceProvider.GetRequiredService<IHubSpotCompanyConnector>();
```

In the sample above, we are assuming that the following environment variables are correctly configured:

- `HubSpot__ClientId`
- `HubSpot__SecretKey`
- `HubSpot__RefreshToken`

## Versioning

This library follows [Semantic Versioning 2.0.0](http://semver.org/spec/v2.0.0.html) for the public releases (published to the [nuget.org](https://www.nuget.org/)).


## How to build

This project uses [Cake](https://cakebuild.net/) as a build engine. You will also need the [.NET Core SDK 3.1.401](https://dotnet.microsoft.com/).

If you would like to build this project locally, just execute the `build.cake` script.

You can do it by using the .NET tool created by CAKE authors and use it to execute the build script.

```powershell
dotnet tool restore
dotnet cake
```