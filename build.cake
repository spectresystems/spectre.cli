#tool nuget:?package=GitVersion.CommandLine&version=3.6.2
#load "./scripts/version.cake"
#load "./scripts/msbuild.cake"
#load "./scripts/appveyor.cake"

var configuration = Argument("configuration", "Release");
var target = Argument("target", "Default");

var ci = AppVeyorSettings.Initialize(Context);
var version = BuildVersion.Calculate(Context, ci);
var settings = MSBuildHelper.CreateSettings(version);

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./.artifacts");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./src/Spectre.CommandLine.sln");
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild("./src/Spectre.CommandLine.sln", new DotNetCoreBuildSettings {
        Configuration = "Release",
        MSBuildSettings = settings
    });
});

Task("Run-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("./src/Spectre.CommandLine.Tests/Spectre.CommandLine.Tests.csproj", new DotNetCoreTestSettings {
        Configuration = "Release"
    });
});

Task("Package")
    .IsDependentOn("Run-Tests")
    .Does(() =>
{
    DotNetCorePack("./src/Spectre.CommandLine/Spectre.CommandLine.csproj", new DotNetCorePackSettings {
        Configuration = "Release",
        OutputDirectory = "./.artifacts",
        MSBuildSettings = settings
    });
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Package")
    .WithCriteria(() => ci.IsRunningOnAppVeyor)
    .Does(() => 
{
    AppVeyor.UploadArtifact(
        new FilePath($"./.artifacts/Spectre.CommandLine.{version.SemVersion}.nupkg")
    );
});

Task("Publish-To-NuGet")
    .IsDependentOn("Package")
    .WithCriteria(() => ci.IsRunningOnAppVeyor 
        && !ci.IsPullRequest
        && (ci.IsDevelopBranch || ci.IsMasterBranch)
        && !ci.IsMaintenanceBuild)
    .Does(() => 
{
    var path = new FilePath($"./.artifacts/Spectre.CommandLine.{version.SemVersion}.nupkg");

    // Get the API key.
    var apiKey = Environment.GetEnvironmentVariable("NUGET_API_KEY");
    if(string.IsNullOrWhiteSpace(apiKey)) {
        throw new CakeException("Could not resolve API key.");
    }

    // Push the package.
    NuGetPush(path, new NuGetPushSettings {
        ApiKey = apiKey,
        Source = "https://nuget.org/api/v2/package"
    });
});

Task("Default")
    .IsDependentOn("Package");

Task("AppVeyor")
    .IsDependentOn("Publish-To-NuGet");

RunTarget(target);