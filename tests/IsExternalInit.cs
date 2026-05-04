#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    // Backfill for older target frameworks/SDKs that don't expose IsExternalInit
    // Allows usage of init-only setters and record types when building against e.g. .NET Framework / netstandard2.0.
    internal static class IsExternalInit
    {
    }
}
#endif
