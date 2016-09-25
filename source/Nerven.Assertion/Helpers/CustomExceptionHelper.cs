using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using JetBrains.Annotations;

namespace Nerven.Assertion.Helpers
{
    public static class CustomExceptionHelper
    {
        private static readonly ConcurrentDictionary<Type, TransformExceptionDelegate> _ExceptionConstructors = new ConcurrentDictionary<Type, TransformExceptionDelegate>();

        [PublicAPI]
        public static bool RegisterConstructor([NotNull] Type exceptionType, [NotNull] TransformExceptionDelegate constructor)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException(nameof(exceptionType));
            }

            if (constructor == null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }

            return _ExceptionConstructors.TryAdd(exceptionType, constructor);
        }

        [PublicAPI]
        public static bool RegisterConstructor<TException>([NotNull] TransformExceptionDelegate constructor)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }

            return _ExceptionConstructors.TryAdd(typeof(TException), constructor);
        }

        [NotNull]
        public static TransformExceptionDelegate CreateCustomExceptionConstructorWithTransform<TException>([CanBeNull] Func<TException, Exception> transformException)
            where TException : Exception
        {
            if (transformException == null)
            {
                return _CreateCustomException<TException>;
            }

            return _innerException => transformException(_CreateCustomException<TException>(_innerException));
        }

        private static TException _CreateCustomException<TException>(MustAssertionException innerException)
            where TException : Exception
        {
            var _constructor = _ExceptionConstructors.GetOrAdd(typeof(TException), _CreateExceptionConstructorSafe);
            return (TException)_constructor(innerException);
        }

        private static TransformExceptionDelegate _CreateExceptionConstructorSafe(Type exceptionType)
        {
            try
            {
                return _CreateExceptionConstructor(exceptionType);
            }
            catch (ArgumentException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
            catch (MemberAccessException)
            {
            }
            catch (NotSupportedException)
            {
            }
            catch (NullReferenceException)
            {
            }
            catch (SecurityException)
            {
            }
            catch (TargetInvocationException)
            {
            }
            catch (TargetParameterCountException)
            {
            }

            return null;
        }

        private static TransformExceptionDelegate _CreateExceptionConstructor(Type exceptionType)
        {
            var _constructor = exceptionType
                .GetTypeInfo()
                .DeclaredConstructors
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
                    IsEmptyConstructor = _c.Parameters.Length == 0
                })
                .OrderByDescending(_c => _c.IsOnlyInnerException)
                .ThenByDescending(_c => _c.IsOnlyMessageAndInnerException)
                .ThenByDescending(_c => _c.IsEmptyConstructor)
                .FirstOrDefault();

            if (_constructor != null && (_constructor.IsOnlyInnerException || _constructor.IsOnlyMessageAndInnerException || _constructor.IsEmptyConstructor))
            {
                var _dynamicMethod = new DynamicMethod(string.Empty, _constructor.Info.DeclaringType, _constructor.Parameters.Select(_p => _p.ParameterType).ToArray(), false);
                var _il = _dynamicMethod.GetILGenerator();

                var _ldargOpCodes = new[] { OpCodes.Ldarg_0, OpCodes.Ldarg_1 };
                for (var _i = 0; _i < _constructor.Parameters.Length; _i++)
                {
                    _il.Emit(_ldargOpCodes[_i]);
                }

                _il.Emit(OpCodes.Newobj, _constructor.Info);
                _il.Emit(OpCodes.Ret);

                TransformExceptionDelegate _result;
                if (_constructor.IsOnlyInnerException)
                {
                    var _delegate = (Func<Exception, Exception>)_dynamicMethod.CreateDelegate(typeof(Func<Exception, Exception>));
                    _result = _innerException => _delegate(_innerException);
                }
                else if (_constructor.IsOnlyMessageAndInnerException)
                {
                    var _delegate = (Func<string, Exception, Exception>)_dynamicMethod.CreateDelegate(typeof(Func<string, Exception, Exception>));
                    _result = _innerException => _delegate(null, _innerException);
                }
                else
                {
                    var _delegate = (Func<Exception>)_dynamicMethod.CreateDelegate(typeof(Func<Exception>));
                    _result = _innerException => _delegate();
                }

                return _result;
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
