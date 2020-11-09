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