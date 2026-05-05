#reference "../../BuildArtifacts/temp/_PublishedLibraries/Cake.ReSharperReports/netstandard2.0/Cake.ReSharperReports.dll"

using Cake.ReSharperReports;

// Self-contained exercise of Cake.ReSharperReports' alias + settings
// surface. The actual ReSharperReports alias shells out to a separate
// tool (ReSharperReports.exe / rsr.exe) installed via #tool, which CI
// doesn't fetch via this script — so this script verifies the addin
// loads, the ReSharperReportsSettings type can be constructed and
// round-tripped, and the alias is callable (it'll throw at
// tool-resolution time, which we catch).

void AssertThat(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception("Assertion failed: " + message);
    }
}

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

    // Also verify the default-constructed shape has both properties null.
    var empty = new ReSharperReportsSettings();
    AssertThat(empty.XslFilePath == null, "Default XslFilePath is null");
    AssertThat(empty.LogFilePath == null, "Default LogFilePath is null");

    Information("ReSharperReportsSettings default constructor OK");
});

Task("Alias-ResolvesToToolError")
    .Does(() =>
{
    // Calling the alias should reach the tool resolution step and fail
    // there with "ReSharperReports could not be located" — that confirms
    // the alias is wired correctly even though the tool isn't installed
    // in CI via this exercise script.
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
