using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    partial class MustAssertableExtensions
    {
        [PublicAPI]
        [AssertionMethod]
        [DebuggerHidden]
        public static IMustAssertable AssumeNever(
            this IMustAssertable must,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            must.Api.NeverAssumptionFailed(
                description: description,
                callerAssembly: Assembly.GetCallingAssembly(),
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);

            return must;
        }
    }
}
