#load nuget:?package=Cake.Recipe&version=4.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                              buildSystem: BuildSystem,
                              sourceDirectoryPath: "./Source",
                              title: "Cake.ReSharperReports",
                              repositoryOwner: "cake-contrib",
                              repositoryName: "Cake.ReSharperReports",
                              appVeyorAccountName: "cakecontrib",
                              shouldRunDotNetCorePack: true,
                              shouldBuildNugetSourcePackage: false,
                              shouldGenerateDocumentation: false,
                              shouldRunCodecov: false,
                              shouldRunInspectCode: false,
                              preferredBuildProviderType: BuildProviderType.GitHubActions);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                             testCoverageFilter: "+[Cake.ReSharperReports]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]*",
                             testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                             testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

Build.RunDotNetCore();
