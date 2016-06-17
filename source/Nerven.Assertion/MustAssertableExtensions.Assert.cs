using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Nerven.Assertion.Helpers;

namespace Nerven.Assertion
{
    partial class MustAssertableExtensions
    {
        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("assertionPassed:false => halt")]
        [DebuggerHidden]
        public static IMustAssertable Assert(
            this IMustAssertable must,
            bool assertionPassed,
            string description,
            TransformExceptionDelegate transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            if (!assertionPassed)
            {
                must.Api.AssertionFailed(
                    description: description,
                    transformException: transformException,
                    callerAssembly: Assembly.GetCallingAssembly(),
                    callerFilePath: callerFilePath,
                    callerLineNumber: callerLineNumber,
                    callerMemberName: callerMemberName);
            }

            return must;
        }

        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("assertionPassed:false => halt")]
        [DebuggerHidden]
        public static IMustAssertable Assert(
            this IMustAssertable must,
            bool assertionPassed,
            TransformExceptionDelegate transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            if (!assertionPassed)
            {
                must.Api.AssertionFailed(
                    description: null,
                    transformException: transformException,
                    callerAssembly: Assembly.GetCallingAssembly(),
                    callerFilePath: callerFilePath,
                    callerLineNumber: callerLineNumber,
                    callerMemberName: callerMemberName);
            }

            return must;
        }

        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("assertionPassed:false => halt")]
        [DebuggerHidden]
        public static IMustAssertable Assert<TException>(
            this IMustAssertable must,
            bool assertionPassed,
            string description,
            Func<TException, Exception> transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
            where TException : Exception
        {
            if (!assertionPassed)
            {
                must.Api.AssertionFailed(
                    description: description,
                    transformException: CustomExceptionHelper.CreateCustomExceptionConstructorWithTransform(transformException),
                    callerAssembly: Assembly.GetCallingAssembly(),
                    callerFilePath: callerFilePath,
                    callerLineNumber: callerLineNumber,
                    callerMemberName: callerMemberName);
            }

            return must;
        }

        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("assertionPassed:false => halt")]
        [DebuggerHidden]
        public static IMustAssertable Assert<TException>(
            this IMustAssertable must,
            bool assertionPassed,
            Func<TException, Exception> transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
            where TException : Exception
        {
            if (!assertionPassed)
            {
                must.Api.AssertionFailed(
                    description: null,
                    transformException: CustomExceptionHelper.CreateCustomExceptionConstructorWithTransform(transformException),
                    callerAssembly: Assembly.GetCallingAssembly(),
                    callerFilePath: callerFilePath,
                    callerLineNumber: callerLineNumber,
                    callerMemberName: callerMemberName);
            }

            return must;
        }
    }
}
