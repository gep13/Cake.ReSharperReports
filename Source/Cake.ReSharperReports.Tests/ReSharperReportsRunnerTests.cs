using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.ReSharperReports.Tests
{
    using System;

    public sealed class ReSharperReportsRunnerTests
    {
        public sealed class TheExecutable
        {
            [Fact]
            public void Should_Throw_If_ReSharperReports_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReSharperReports: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("/bin/tools/ReSharperReports/ReSharperReports.exe", "/bin/tools/ReSharperReports/ReSharperReports.exe")]
            [InlineData("./tools/ReSharperReports/ReSharperReports.exe", "/Working/tools/ReSharperReports/ReSharperReports.exe")]
            public void Should_Use_ReSharperReports_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture { Settings = { ToolPath = toolPath } };
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/ReSharperReports/ReSharperReports.exe", "C:/ReSharperReports/ReSharperReports.exe")]
            public void Should_Use_ReSharperReports_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture { Settings = { ToolPath = toolPath } };
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_ReSharperReports_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/ReSharperReports.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Input_File_Is_Null()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture();
                fixture.InputFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("inputFilePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Output_File_Is_Null()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture();
                fixture.OutputFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("outputFilePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReSharperReports: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReSharperReports: Process returned an error (exit code 1).", result.Message);
            }

            [WindowsFact]
            public void Should_Set_Xsl_File()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture { Settings = { XslFilePath = "C:/temp/input.xsl" } };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("transform -i \"c:/temp/input.xml\" -o \"c:/temp/output.html\" -x \"C:/temp/input.xsl\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_Log_File()
            {
                // Given
                var fixture = new ReSharperReportsRunnerFixture { Settings = { LogFilePath = "C:/temp/output.log" } };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("transform -i \"c:/temp/input.xml\" -o \"c:/temp/output.html\" -l \"C:/temp/output.log\"", result.Args);
            }
        }
    }
}