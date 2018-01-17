var target = Argument<string>("Target", "Build");

Task("Build")
    .Does(() =>
{
    DotNetCoreBuild("HubSpotSdk.sln");
});

RunTarget(target);