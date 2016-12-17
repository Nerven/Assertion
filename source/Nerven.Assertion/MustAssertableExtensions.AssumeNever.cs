using System.Diagnostics;
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
            if (!must.Api.EvaluateAssumptions)
                return must;

            must.Api.NeverAssumptionFailed(
                description: description,
                callerAssembly: null,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);

            return must;
        }
    }
}
