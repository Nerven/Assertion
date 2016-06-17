using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Nerven.Assertion.Helpers
{
    public static class ExceptionHelper
    {
        [CanBeNull]
        public static Exception Combine([CanBeNull] params IEnumerable<Exception>[] exceptionCollections)
        {
            var _exceptions = exceptionCollections?
                .SelectMany(_exceptionCollection => _exceptionCollection?.Where(_exception => _exception != null) ?? Enumerable.Empty<Exception>())
                .ToList();

            switch (_exceptions?.Count ?? 0)
            {
                case 0:
                    return null;
                case 1:
                    return _exceptions?[0];
                default:
                    return new AggregateException(_exceptions);
            }
        }

        [CanBeNull]
        public static Exception Combine([CanBeNull] params Exception[] exceptions)
        {
            if (exceptions == null)
                return null;

            var _length = exceptions.Length;
            var _exceptionCollections = new IEnumerable<Exception>[_length];
            for (var _i = 0; _i < _length; _i++)
            {
                _exceptionCollections[_i] = Yield(exceptions[_i]);
            }

            return Combine(_exceptionCollections);
        }

        [CanBeNull]
        public static IEnumerable<Exception> Yield([CanBeNull] Exception exception)
        {
            if (exception != null)
            {
                yield return exception;
            }
        }
    }
}
