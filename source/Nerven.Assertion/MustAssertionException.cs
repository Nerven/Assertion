using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    [PublicAPI]
    [Serializable]
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
            : this(_BuildExceptionMessage(assertionRecord), innerException)
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

        protected MustAssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            throw new NotImplementedException();
        }

        public MustAssertionRecord AssertionRecord { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(AssertionRecord), AssertionRecord);
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return _BuildExceptionMessage(AssertionRecord);
        }

        private static string _BuildExceptionMessage(MustAssertionRecord assertionRecord)
        {
            var _newLine = Environment.NewLine;
            var _s = assertionRecord?.Description == null ? "Assertion failed" : $"Assertion failed: Asserts that {assertionRecord.Description}";

            if (assertionRecord?.CallerFilePath != null)
            {
                _s += $"{_newLine}Location: {assertionRecord.CallerFilePath}";

                if (assertionRecord.CallerLineNumber.GetValueOrDefault() != 0)
                {
                    _s += $":{assertionRecord.CallerLineNumber}";
                }
            }

            if (assertionRecord?.CallerMemberName != null)
            {
                _s += $"{_newLine}Member: {assertionRecord.CallerMemberName}";
            }

            if (assertionRecord?.Data?.Count > 0)
            {
                foreach (var _data in assertionRecord.Data)
                {
                    string _dataString;
                    try
                    {
                        _dataString = _data.Value?.ToString() ?? "<null>";
                    }
                    catch (Exception _exception)
                    {
                        _dataString = $"<unrepresentable: {_exception.Message}>";
                    }

                    _s += $"{_newLine}{_data.Key} <{_data.Type}>: {_dataString}";
                }
            }

            return _s;
        }
    }
}
