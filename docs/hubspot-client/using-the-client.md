Once configured, it's possible to get an instance of the client using the Dependency Injection subsystem.

This means you can either request the client using the constructor of a calling service or directly from an instance of `IServiceProvider`.

```csharp
// Indirectly from the calling service's constructor
public class MyService(IHubSpotClient client)
{
    // ... 
}

// Directly form an instance of IServiceProvider
var serviceProvider = services.BuildServiceProvider();

var client = serviceProvider.GetRequiredService<IHubSpotClient>();
```

To avoid overloading the user with the many methods offered by the HubSpot REST API, the methods are divided into categories following the definition of [HubSpot reference](https://developers.hubspot.com/docs/api/overview).

Please note that not all categories and methods have been added to the library yet.

Here is the list of APIs currently available, divided by categories:

- Contacts
  - Properties
  - Property groups
- Companies
- Deals
- Lists
- Owners
- CRM
  - Associations

For example, to get a company by its HubSpot ID, you could do:

```csharp
var company = await client.Companies.GetByIdAsync(companyId);
```