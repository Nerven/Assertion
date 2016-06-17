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
        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        public static IMustAssertable AssertNever(
            this IMustAssertable must,
            string description,
            TransformExceptionDelegate transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            must.Api.NeverAssertionFailed(
                description: description,
                transformException: transformException,
                callerAssembly: Assembly.GetCallingAssembly(),
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);

            // This cannot happen
            return null;
        }

        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        public static IMustAssertable AssertNever(
            this IMustAssertable must,
            TransformExceptionDelegate transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            must.Api.NeverAssertionFailed(
                description: null,
                transformException: transformException,
                callerAssembly: Assembly.GetCallingAssembly(),
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);

            // This cannot happen
            return null;
        }

        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        public static MustAssertionException AssertNever<TException>(
            this IMustAssertable must,
            string description,
            Func<TException, Exception> transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
            where TException : Exception, new()
        {
            must.Api.NeverAssertionFailed(
                description: description,
                transformException: CustomExceptionHelper.CreateCustomExceptionConstructorWithTransform(transformException),
                callerAssembly: Assembly.GetCallingAssembly(),
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);

            // This cannot happen
            return null;
        }

        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        public static MustAssertionException AssertNever<TException>(
            this IMustAssertable must,
            Func<TException, Exception> transformException = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
            where TException : Exception, new()
        {
            must.Api.NeverAssertionFailed(
                description: null,
                transformException: CustomExceptionHelper.CreateCustomExceptionConstructorWithTransform(transformException),
                callerAssembly: Assembly.GetCallingAssembly(),
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);

            // This cannot happen
            return null;
        }
    }
}
