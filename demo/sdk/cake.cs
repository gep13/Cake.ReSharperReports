#!/usr/bin/env dotnet
#:sdk Cake.Sdk@6.1.1
#:project ../../Source/Cake.ReSharperReports/Cake.ReSharperReports.csproj
#:property Nullable=enable

// Nullable=enable propagates `<Nullable>enable</Nullable>` to the
// SDK's synthetic csproj so the addin's `FilePath?` annotations on
// ReSharperReportsSettings load in the right context (silences
// CS8632).
//
// NoWarn=CS8603 silences "Possible null reference return" warnings
// emitted from Cake.Sdk's generated `CakeMethodAliases.g.cs` — the
// source generator doesn't currently propagate the addin's `?`
// annotations into the synthesized wrappers, so it reports the
// nullable returns as suspicious. Not actionable from our code;
// upstream issue against Cake.Sdk.
#:property NoWarn=CS8603

// Cake SDK consumer demo for Cake.ReSharperReports. Runs as a
// file-based .NET program (introduced in .NET 10) using the
// Cake.Sdk directives. The #:project directive above lets the SDK
// build the addin from source rather than referencing a published
// nupkg.
//
// To run locally:
//   cd demo/sdk
//   dotnet cake.cs
//
// Runs the same two checks the script and frosting demos run.

using Cake.ReSharperReports;

Task("Default")
    .IsDependentOn("Settings-Roundtrip")
    .IsDependentOn("Alias-ResolvesToToolError");

Task("Settings-Roundtrip")
    .Does(() =>
{
    var settings = new ReSharperReportsSettings
    {
        XslFilePath = File("./xsl/dupfinder.xsl"),
        LogFilePath = File("./logs/resharperreports.log"),
    };

    AssertThat(settings.XslFilePath != null && settings.XslFilePath.FullPath.EndsWith("dupfinder.xsl"), "XslFilePath roundtrip");
    AssertThat(settings.LogFilePath != null && settings.LogFilePath.FullPath.EndsWith("resharperreports.log"), "LogFilePath roundtrip");

    Information("ReSharperReportsSettings OK (both 2 properties round-tripped)");

    var empty = new ReSharperReportsSettings();
    AssertThat(empty.XslFilePath == null, "Default XslFilePath is null");
    AssertThat(empty.LogFilePath == null, "Default LogFilePath is null");

    Information("ReSharperReportsSettings default constructor OK");
});

Task("Alias-ResolvesToToolError")
    .Does(() =>
{
    var threw = false;
    try
    {
        ReSharperReports(File("./fake-input.xml"), File("./fake-output.html"));
    }
    catch (Exception ex) when (ex.Message.IndexOf("ReSharperReports", StringComparison.OrdinalIgnoreCase) >= 0
                              || ex.Message.IndexOf("not be found", StringComparison.OrdinalIgnoreCase) >= 0
                              || ex.Message.IndexOf("could not locate", StringComparison.OrdinalIgnoreCase) >= 0)
    {
        threw = true;
        Information("Alias resolved correctly; tool-not-found exception was: {0}", ex.Message);
    }

    AssertThat(threw, "Expected ReSharperReports alias to throw a tool-not-found exception (ReSharperReports.exe is installed via #tool and not present in CI)");
});

RunTarget("Default");

// ----- Helpers (must come AFTER top-level statements per CS8803) -----

static void AssertThat(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception("Assertion failed: " + message);
    }
}
