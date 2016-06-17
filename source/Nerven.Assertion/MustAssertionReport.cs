using System;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    public sealed class MustAssertionReport
    {
        public MustAssertionReport(DateTimeOffset timestamp, MustAssertionType assertionType, MustAssertionRecord assertionRecord)
        {
            Timestamp = timestamp;
            AssertionType = assertionType;
            AssertionRecord = assertionRecord;
        }

        [PublicAPI]
        public DateTimeOffset Timestamp { get; }

        [PublicAPI]
        public MustAssertionType AssertionType { get; }

        [PublicAPI]
        public MustAssertionRecord AssertionRecord { get; }
    }
}
