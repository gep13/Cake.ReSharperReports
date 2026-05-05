using System;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;
using Cake.ReSharperReports;

namespace Build.Tasks
{
    [TaskName("Alias-ResolvesToToolError")]
    public sealed class AliasResolvesToToolErrorTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            // Calling the alias should reach the tool resolution step and
            // fail there with "ReSharperReports could not be located" — that
            // confirms the alias is wired correctly even though the tool
            // isn't installed in CI via this exercise harness.
            var threw = false;
            try
            {
                context.ReSharperReports(context.File("./fake-input.xml"), context.File("./fake-output.html"));
            }
            catch (Exception ex) when (ex.Message.IndexOf("ReSharperReports", StringComparison.OrdinalIgnoreCase) >= 0
                                       || ex.Message.IndexOf("not be found", StringComparison.OrdinalIgnoreCase) >= 0
                                       || ex.Message.IndexOf("could not locate", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                threw = true;
                context.Information("Alias resolved correctly; tool-not-found exception was: {0}", ex.Message);
            }

            if (!threw)
            {
                throw new Exception("Expected ReSharperReports alias to throw a tool-not-found exception");
            }
        }
    }
}
