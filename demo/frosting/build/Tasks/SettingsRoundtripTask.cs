using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;
using Cake.ReSharperReports;

namespace Build.Tasks
{
    [TaskName("Settings-Roundtrip")]
    public sealed class SettingsRoundtripTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new ReSharperReportsSettings
            {
                XslFilePath = context.File("./xsl/dupfinder.xsl"),
                LogFilePath = context.File("./logs/resharperreports.log"),
            };

            if (settings.XslFilePath == null || !settings.XslFilePath.FullPath.EndsWith("dupfinder.xsl"))
            {
                throw new System.Exception("XslFilePath round-trip failed");
            }

            if (settings.LogFilePath == null || !settings.LogFilePath.FullPath.EndsWith("resharperreports.log"))
            {
                throw new System.Exception("LogFilePath round-trip failed");
            }

            context.Information("ReSharperReportsSettings OK (both 2 properties round-tripped)");

            var empty = new ReSharperReportsSettings();
            if (empty.XslFilePath != null)
            {
                throw new System.Exception("Default XslFilePath should be null");
            }

            if (empty.LogFilePath != null)
            {
                throw new System.Exception("Default LogFilePath should be null");
            }

            context.Information("ReSharperReportsSettings default constructor OK");
        }
    }
}
