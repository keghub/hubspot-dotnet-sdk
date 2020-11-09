Another feature offered by connectors is the ability to use custom serializers to persist complex objects into HubSpot custom properties.

In case of need, simply create a class implementing the interface `ITypeConverter` and register it at startup.

Here is an example on how to register a custom converter.

```csharp
services.AddHubSpot(hs => hs
        .RegisterConverter(new MyCustomTypeConverter(), typeof(MyCustomType))
        .UseCompanyConnector()
        .UseContactConnector());
```