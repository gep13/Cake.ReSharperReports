using System.Runtime.InteropServices;
using Xunit;

namespace Cake.ReSharperReports.Tests
{
    /// <summary>
    /// xunit Fact that runs only on Windows. The Cake path-resolution model
    /// only recognises drive-letter prefixes ("C:/...") as absolute on
    /// Windows; on Linux it treats the same string as relative and prepends
    /// the working directory, which changes the assertion outcome. Tests
    /// that pin Windows-style absolute paths in their expected values must
    /// use this attribute.
    /// </summary>
    public sealed class WindowsFactAttribute : FactAttribute
    {
        public WindowsFactAttribute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Skip = "Windows-only — Cake treats drive-letter paths as relative on non-Windows.";
            }
        }
    }

    /// <summary>
    /// xunit Theory that runs only on Windows. See <see cref="WindowsFactAttribute"/>.
    /// </summary>
    public sealed class WindowsTheoryAttribute : TheoryAttribute
    {
        public WindowsTheoryAttribute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Skip = "Windows-only — Cake treats drive-letter paths as relative on non-Windows.";
            }
        }
    }
}
