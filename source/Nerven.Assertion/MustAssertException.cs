using System;
using System.Runtime.Serialization;

namespace Nerven.Assertion
{
    [Serializable]
    public class MustAssertException : Exception
    {
        public MustAssertException(
            string message = null,
            Exception innerException = null,
            string description = null,
            string callerFilePath = null,
            int callerLineNumber = 0,
            string callerMemberName = null)
            : base(message ?? (description == null ? "Assertion failed" : "Assertion '" + description + "' failed"), innerException)
        {
            Description = description;
            CallerFilePath = callerFilePath;
            CallerLineNumber = callerLineNumber;
            CallerMemberName = callerMemberName;
        }

        //// ReSharper disable once RedundantArgumentDefaultValue
        public MustAssertException()
            : this(null)
        {
        }

        public MustAssertException(
            string description,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName)
            : this(null, null, description, callerFilePath, callerLineNumber, callerMemberName)
        {
        }

        protected MustAssertException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                Description = info.GetString(nameof(Description));
                CallerFilePath = info.GetString(nameof(CallerFilePath));
                CallerLineNumber = info.GetInt32(nameof(CallerLineNumber));
                CallerMemberName = info.GetString(nameof(CallerMemberName));
            }
        }

        public string Description { get; }

        public string CallerFilePath { get; }

        public int CallerLineNumber { get; }

        public string CallerMemberName { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(Description), Description);
            info.AddValue(nameof(CallerFilePath), CallerFilePath);
            info.AddValue(nameof(CallerLineNumber), CallerLineNumber);
            info.AddValue(nameof(CallerMemberName), CallerMemberName);
        }

        public override string ToString()
        {
            var _s = typeof(MustAssertException).FullName + ": ";

            _s += Message;

            if (CallerFilePath != null)
            {
                _s += Environment.NewLine;

                _s += "Location: " + CallerFilePath;

                if (CallerLineNumber > 0)
                {
                    _s += ":" + CallerLineNumber;
                }
            }

            if (CallerMemberName != null)
            {
                _s += Environment.NewLine;
                _s += "Member: " + CallerMemberName;
            }

            return _s;
        }
    }
}
