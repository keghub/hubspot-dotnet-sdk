# EMG .NET SDK for HubSpot

This repository contains an SDK for .NET applications needing to integrate with [HubSpot](https://www.hubspot.com).

This SDK is composed of two packages:

- `EMG.HubSpot.Client`,
- `EMG.HubSpot`

You can find detailed information about how to use this library in the [documentation site](https://docs.educationsmediagroup.com/hubspot-sdk/).

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
