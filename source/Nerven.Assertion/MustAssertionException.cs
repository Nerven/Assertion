using System;
using JetBrains.Annotations;
using Nerven.Assertion.Helpers;

namespace Nerven.Assertion
{
    [PublicAPI]
    public class MustAssertionException : Exception
    {
        public MustAssertionException(
            string message = null,
            Exception innerException = null)
            : base(message, innerException)
        {
        }

        public MustAssertionException(
            Exception innerException,
            MustAssertionRecord assertionRecord)
            : this(AssertionRecordHelper.BuildMessage(assertionRecord), innerException)
        {
            AssertionRecord = assertionRecord;
        }

        public MustAssertionException(
            MustAssertionRecord assertionRecord)
            : this(null, assertionRecord)
        {
            AssertionRecord = assertionRecord;
        }

        //// ReSharper disable once RedundantArgumentDefaultValue
        public MustAssertionException()
            : this(null)
        {
        }

        public MustAssertionRecord AssertionRecord { get; }
    }
}
