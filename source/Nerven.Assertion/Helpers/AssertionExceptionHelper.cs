using System;
using JetBrains.Annotations;

namespace Nerven.Assertion.Helpers
{
    public static class AssertionExceptionHelper
    {
        public static Exception CreateAssertionException(
            [CanBeNull] [InstantHandle] TransformExceptionDelegate transformException,
            [CanBeNull] MustAssertionRecord assertionRecord,
            [CanBeNull] params Exception[] additionalExceptions)
        {
            var _assertionException = new MustAssertionException(assertionRecord);

            Exception _transformedException;
            Exception _transformExceptionErrorException;
            if (transformException == null)
            {
                _transformedException = _assertionException;
                _transformExceptionErrorException = null;
            }
            else
            {
                try
                {
                    _transformedException = transformException(_assertionException) ?? _assertionException;
                    _transformExceptionErrorException = null;
                }
                catch (Exception _exception)
                {
                    _transformedException = _assertionException;
                    _transformExceptionErrorException = _exception;
                }
            }

            var _finalException = ExceptionHelper.Combine(
                ExceptionHelper.Yield(_transformedException),
                ExceptionHelper.Yield(_transformExceptionErrorException),
                additionalExceptions);
            return _finalException;
        }
    }
}
