using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    public sealed class MustAssert
    {
        private static readonly ConcurrentDictionary<Type, Func<MustAssertException, Exception>> _ExceptionConstructors = new ConcurrentDictionary<Type, Func<MustAssertException, Exception>>();

        private MustAssert()
        {
        }

        internal static MustAssert _Instance { get; } = new MustAssert();

        //// ReSharper disable ExplicitCallerInfoArgument
        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public MustAssert Assert(
            bool assertionResult,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
        {
            return Assert(
                assertionResult,
                _e => _e,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public MustAssert Assert<TException>(
            bool assertionResult,
            string description = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMemberName = null)
            where TException : Exception, new()
        {
            return Assert(
                assertionResult,
                _InitializeException<TException>,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public MustAssert Assert(
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

            return Assert(
                assertionResult,
                _innerException => newException(),
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }
        //// ReSharper restore ExplicitCallerInfoArgument

        [ContractAnnotation("assertionResult:false => halt")]
        [DebuggerHidden]
        public MustAssert Assert(
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

            if (!assertionResult)
            {
                throw _GetException(
                    newException,
                    description: description,
                    callerFilePath: callerFilePath,
                    callerLineNumber: callerLineNumber,
                    callerMemberName: callerMemberName);
            }

            return this;
        }

        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        internal Exception _Never(
            string description,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName)
        {
            return _Never(
                _e => _e,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        internal Exception _Never<TException>(
            string description,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName)
            where TException : Exception, new()
        {
            return _Never(
                _InitializeException<TException>,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        internal Exception _Never(
            [InstantHandle] Func<Exception> newException,
            string description,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName)
        {
            return _Never(
                _innerException => newException(),
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        internal Exception _Never(
            [InstantHandle] Func<MustAssertException, Exception> newException,
            string description,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName)
        {
            throw _GetException(
                newException,
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);
        }

        private Exception _GetException(
            [InstantHandle] Func<MustAssertException, Exception> newException,
            string description,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName)
        {
            if (newException == null)
            {
                throw new ArgumentNullException(nameof(newException));
            }

            var _innerException = new MustAssertException(
                description: description,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                callerMemberName: callerMemberName);

            return newException(_innerException) ?? _innerException;
        }

        private static TException _InitializeException<TException>(MustAssertException innerException)
            where TException : Exception, new()
        {
            var _constructor = _ExceptionConstructors.GetOrAdd(typeof(TException), _CreateExceptionConstructorSafe);

            if (_constructor == null)
            {
                return new TException();
            }

            return (TException)_constructor(innerException);
        }

        private static Func<MustAssertException, Exception> _CreateExceptionConstructorSafe(Type exceptionType)
        {
            try
            {
                return _CreateExceptionConstructor(exceptionType);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            catch (MemberAccessException)
            {
                return null;
            }
            catch (NotSupportedException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (SecurityException)
            {
                return null;
            }
            catch (TargetInvocationException)
            {
                return null;
            }
            catch (TargetParameterCountException)
            {
                return null;
            }
        }

        private static Func<MustAssertException, Exception> _CreateExceptionConstructor(Type exceptionType)
        {
            var _constructor = exceptionType.GetConstructors()
                    .Select(_c => new
                    {
                        Info = _c,
                        Parameters = _c.GetParameters(),
                    })
                .Select(_c => new
                    {
                        _c.Info,
                        _c.Parameters,
                        IsOnlyInnerException = _c.Parameters.Length == 1 && _IsParameterAnInnerExceptionParameter(_c.Parameters[0]),
                        IsOnlyMessageAndInnerException = _c.Parameters.Length == 2 && _IsParameterAMessageParameter(_c.Parameters[0]) && _IsParameterAnInnerExceptionParameter(_c.Parameters[1]),
                    })
                .OrderByDescending(_c => _c.IsOnlyInnerException)
                .ThenByDescending(_c => _c.IsOnlyMessageAndInnerException)
                .FirstOrDefault();

            if (_constructor != null && (_constructor.IsOnlyInnerException || _constructor.IsOnlyMessageAndInnerException))
            {
                var _dynamicMethod = new DynamicMethod(string.Empty, _constructor.Info.DeclaringType, Array.ConvertAll(_constructor.Parameters, _p => _p.ParameterType), false);

                var _il = _dynamicMethod.GetILGenerator();
                _il.Emit(OpCodes.Ldarg_0);

                if (!_constructor.IsOnlyInnerException)
                {
                    _il.Emit(OpCodes.Ldarg_1);
                }

                _il.Emit(OpCodes.Newobj, _constructor.Info);
                _il.Emit(OpCodes.Ret);

                if (_constructor.IsOnlyInnerException)
                {
                    return (Func<Exception, Exception>)_dynamicMethod.CreateDelegate(typeof(Func<Exception, Exception>));
                }

                return _innerException => ((Func<string, Exception, Exception>)_dynamicMethod.CreateDelegate(typeof(Func<string, Exception, Exception>)))(null, _innerException);
            }

            return null;
        }

        private static bool _IsParameterAnInnerExceptionParameter(ParameterInfo parameter)
        {
            return parameter != null && parameter.Name == "innerException" && parameter.ParameterType == typeof(Exception);
        }

        private static bool _IsParameterAMessageParameter(ParameterInfo parameter)
        {
            return parameter != null && parameter.Name == "message" && parameter.ParameterType == typeof(string);
        }
    }
}
