using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    public sealed class MustAssertionReport
    {
        public MustAssertionReport(DateTimeOffset timestamp, MustAssertionType assertionType, MustAssertionRecord assertionRecord)
        {
            if (assertionRecord == null)
            {
                throw new ArgumentNullException(nameof(assertionRecord));
            }

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

        public override string ToString()
        {
            return $"{AssertionType} {Timestamp.ToString(CultureInfo.InvariantCulture)}: {AssertionRecord}";
        }
    }
}
