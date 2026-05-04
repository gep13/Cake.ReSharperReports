using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.ReSharperReports
{
    /// <summary>
    /// <para>Contains functionality related to the <see href="https://github.com/gep13/ReSharperReports">ReSharperReports</see> tool.</para>
    /// <para>
    /// In order to use the commands for this addin, you will need to include the following in your build.cake file to download and reference from NuGet.org:
    /// <code>
    /// #tool ReSharperReports
    /// </code>
    /// In addition, you will need to include the following:
    /// <code>
    /// #addin Cake.ReSharperReports
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("ReSharperReports")]
    public static class ReSharperReportsAliases
    {
        /// <summary>
        /// Runs ReSharperReports against the specified input FilePath, and outputs to specified output FilePath.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <example>
        /// <code>
        /// ReSharperReports("C:/temp/dupfinder-output.xml", "c:/temp/dupfinder-output.html");
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// ReSharperReports("C:/temp/inspectcode-output.xml", "c:/temp/inspectcode-output.html");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReSharperReports(this ICakeContext context, FilePath inputFilePath, FilePath outputFilePath)
        {
            ReSharperReports(context, inputFilePath, outputFilePath, new ReSharperReportsSettings());
        }

        /// <summary>
        /// Runs ReSharperReports against the specified input FilePath, and outputs to specified output FilePath with the specified ReSharperReportsSettings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ReSharperReports("C:/temp/dupfinder-output.xml", "c:/temp/dupfinder-output-xsl.html", new ReSharperReportsSettings()
        /// {
        ///     XslFilePath = "c:/temp/dupfinder.xsl"
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// ReSharperReports("C:/temp/inspectcode-output.xml", "c:/temp/inspectcode-output-xsl.html", new ReSharperReportsSettings()
        /// {
        ///     XslFilePath = "c:/temp/inspectcode.xsl"
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// ReSharperReports("C:/temp/dupfinder-output.xml", "c:/temp/dupfinder-output-xsl.html", new ReSharperReportsSettings()
        /// {
        ///     XslFilePath = "c:/temp/dupfinder.xsl",
        ///     LogFilePath = "c:/temp/resharperreports-dupfinder.log"
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// ReSharperReports("C:/temp/inspectcode-output.xml", "c:/temp/inspectcode-output-xsl.html", new ReSharperReportsSettings()
        /// {
        ///     XslFilePath = "c:/temp/inspectcode.xsl",
        ///     LogFilePath = "c:/temp/resharperreports-inspectcode.log"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReSharperReports(this ICakeContext context, FilePath inputFilePath, FilePath outputFilePath, ReSharperReportsSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (inputFilePath == null)
            {
                throw new ArgumentNullException("inputFilePath");
            }

            if (outputFilePath == null)
            {
                throw new ArgumentNullException("outputFilePath");
            }

            var runner = new ReSharperReportsRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(inputFilePath, outputFilePath, settings);
        }
    }
}