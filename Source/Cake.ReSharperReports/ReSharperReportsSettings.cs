using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.ReSharperReports
{
    /// <summary>
    /// Contains settings used by <see cref="ReSharperReportsRunner"/>.
    /// </summary>
    public sealed class ReSharperReportsSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the XSL Transform File path.
        /// </summary>
        /// <value>The XSL Transform File path.</value>
        public FilePath? XslFilePath { get; set; }

        /// <summary>
        /// Gets or sets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public FilePath? LogFilePath { get; set; }
    }
}