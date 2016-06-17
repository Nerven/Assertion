using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Nerven.Assertion.Helpers
{
    public static class AssertionRecordHelper
    {
        public static MustAssertionRecord CreateRecord(
            [CanBeNull] IReadOnlyList<MustAssertionData> data,
            [CanBeNull] string description,
            [CanBeNull] Assembly callerAssembly,
            [CanBeNull] string callerFilePath,
            [CanBeNull] int? callerLineNumber,
            [CanBeNull] string callerMemberName)
        {
            var _assemblyFullName = callerAssembly?.FullName;
            var _assemblyVersion = callerAssembly?.GetName().Version.ToString();

            return new MustAssertionRecord(
                data,
                description,
                callerFilePath,
                callerLineNumber,
                callerMemberName,
                _assemblyFullName,
                _assemblyVersion);
        }
    }
}
