using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

//// ReSharper disable ExplicitCallerInfoArgument

namespace Nerven.Assertion
{
    public static class Must
    {
        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public static MustAssert Assert(
            bool assertionResult,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            return MustAssert._Instance.Assert(
                assertionResult,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public static MustAssert Assert<TException>(
            bool assertionResult,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
            where TException : Exception, new()
        {
            return MustAssert._Instance.Assert<TException>(
                assertionResult,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public static MustAssert Assert(
            bool assertionResult,
            [InstantHandle] Func<Exception> newException,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            if (newException == null)
            {
                throw new ArgumentNullException(nameof(newException));
            }

            return MustAssert._Instance.Assert(
                assertionResult,
                newException,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public static MustAssert Assert(
            bool assertionResult,
            [InstantHandle] Func<MustAssertException, Exception> newException,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            if (newException == null)
            {
                throw new ArgumentNullException(nameof(newException));
            }

            return MustAssert._Instance.Assert(
                assertionResult,
                newException,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        public static Exception Never(
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            throw new MustAssertException(
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }
    }
}
