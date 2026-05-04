using Build.Tasks;
using Cake.Frosting;

namespace Build
{
    [TaskName("Default")]
    [IsDependentOn(typeof(SettingsRoundtripTask))]
    [IsDependentOn(typeof(AliasResolvesToToolErrorTask))]
    public sealed class DefaultTask : FrostingTask
    {
    }
}
