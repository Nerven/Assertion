using System;
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
        public static IMustAssertable Assume(
            this IMustAssertable must,
            [NotNull] Func<bool> getAsumptionPassed,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (getAsumptionPassed == null || !getAsumptionPassed())
            {
                must.Api.AssumptionFailed(
                    description: description,
                    callerAssembly: Assembly.GetCallingAssembly(),
                    callerFilePath: callerFilePath,
                    callerLineNumber: callerLineNumber,
                    callerMemberName: callerMemberName);
            }

            return must;
        }
    }
}
