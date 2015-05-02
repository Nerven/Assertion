using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

//// ReSharper disable ExplicitCallerInfoArgument

namespace Nerven.Assertion
{
    public static class Must
    {
        [DebuggerHidden]
        public static MustAssert Assert(
            [InstantHandle] Func<bool> assertion,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            if (assertion == null)
            {
                throw new ArgumentNullException(nameof(assertion));
            }

            return MustAssert.Instance.Assert(
                assertion,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public static MustAssert Assert(
            bool assertionResult,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            return MustAssert.Instance.Assert(
                assertionResult,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [DebuggerHidden]
        public static MustAssert Assert<TException>(
            [InstantHandle] Func<bool> assertion,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
            where TException : Exception, new()
        {
            if (assertion == null)
            {
                throw new ArgumentNullException(nameof(assertion));
            }

            return MustAssert.Instance.Assert<TException>(
                assertion,
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
            return MustAssert.Instance.Assert<TException>(
                assertionResult,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [DebuggerHidden]
        public static MustAssert Assert(
            [InstantHandle] Func<bool> assertion,
            [InstantHandle] Func<Exception> newException,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            if (assertion == null)
            {
                throw new ArgumentNullException(nameof(assertion));
            }

            if (newException == null)
            {
                throw new ArgumentNullException(nameof(newException));
            }

            return MustAssert.Instance.Assert(
                assertion,
                newException,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [DebuggerHidden]
        public static MustAssert Assert(
            [InstantHandle] Func<bool> assertion,
            [InstantHandle] Func<MustAssertException, Exception> newException,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            if (assertion == null)
            {
                throw new ArgumentNullException(nameof(assertion));
            }

            if (newException == null)
            {
                throw new ArgumentNullException(nameof(newException));
            }

            return MustAssert.Instance.Assert(
                assertion,
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

            return MustAssert.Instance.Assert(
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

            return MustAssert.Instance.Assert(
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
