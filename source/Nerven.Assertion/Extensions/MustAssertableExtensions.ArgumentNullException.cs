using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Nerven.Assertion.Helpers;

namespace Nerven.Assertion.Extensions
{
    public static partial class MustAssertableExtensions
    {
        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("value:null => halt")]
        [DebuggerHidden]
        public static IMustAssertable AssertArgumentNotNull<T>(
            this IMustAssertable must,
            T value,
            [InvokerParameterName] string parameterName)
            where T : class
        {
            return must.Assert(
                value != null,
                //// ReSharper disable once AssignNullToNotNullAttribute
                _innerException => ExceptionHelper.Combine(new ArgumentNullException(parameterName), _innerException));
        }

        [PublicAPI]
        [AssertionMethod]
        [ContractAnnotation("value:null => halt")]
        [DebuggerHidden]
        public static IMustAssertable AssertNotNull<T>(
            this IMustAssertable must,
            T value)
            where T : class
        {
            return must.Assert(
                value != null,
                _innerException => new ArgumentNullException(null, _innerException));
        }
    }
}
