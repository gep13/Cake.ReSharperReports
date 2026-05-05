using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ReSharperReports
{
    /// <summary>
    /// The ReSharperReports runner.
    /// </summary>
    public sealed class ReSharperReportsRunner : Tool<ReSharperReportsSettings>
    {
        private readonly ICakeEnvironment environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReSharperReportsRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool locator.</param>
        public ReSharperReportsRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolLocator)
            : base(fileSystem, environment, processRunner, toolLocator)
        {
            this.environment = environment;
        }

        /// <summary>
        /// Transform the input XML file using either default XSL file, or one passed in via Settings.
        /// </summary>
        /// <param name="inputFilePath">The input filepath.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath inputFilePath, FilePath outputFilePath, ReSharperReportsSettings settings)
        {
            if (inputFilePath == null)
            {
                throw new ArgumentNullException(nameof(inputFilePath));
            }

            if (outputFilePath == null)
            {
                throw new ArgumentNullException(nameof(outputFilePath));
            }

            settings = settings ?? new ReSharperReportsSettings();

            Run(settings, GetArguments(inputFilePath, outputFilePath, settings));
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "ReSharperReports";
        }

        /// <summary>
        /// Gets the name of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "ReSharperReports.exe", "rsr.exe" };
        }

        private ProcessArgumentBuilder GetArguments(FilePath inputFilePath, FilePath outputFilePath, ReSharperReportsSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("transform");

            builder.Append("-i");
            builder.AppendQuoted(inputFilePath.MakeAbsolute(environment).FullPath);

            builder.Append("-o");
            builder.AppendQuoted(outputFilePath.MakeAbsolute(environment).FullPath);

            if (settings.XslFilePath != null)
            {
                builder.Append("-x");
                builder.AppendQuoted(settings.XslFilePath.MakeAbsolute(environment).FullPath);
            }

            if (settings.LogFilePath != null)
            {
                builder.Append("-l");
                builder.AppendQuoted(settings.LogFilePath.MakeAbsolute(environment).FullPath);
            }

            return builder;
        }
    }
}