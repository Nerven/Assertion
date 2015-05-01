using System;
using System.Runtime.Serialization;

namespace Nerven.Assertion
{
    [Serializable]
    public class MustAssertException : Exception
    {
        private readonly string _Description;
        private readonly string _CallerFilePath;
        private readonly int _CallerLineNumber;
        private readonly string _CallerMemberName;

        public MustAssertException(
            string message = null,
            Exception innerException = null,
            string description = null,
            string callerFilePath = null,
            int callerLineNumber = 0,
            string callerMemberName = null)
            : base(message ?? (description == null ? "Assertion failed" : "Assertion '" + description + "' failed"), innerException)
        {
            _Description = description;
            _CallerFilePath = callerFilePath;
            _CallerLineNumber = callerLineNumber;
            _CallerMemberName = callerMemberName;
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
                _Description = info.GetString("Description");
                _CallerFilePath = info.GetString("CallerFilePath");
                _CallerLineNumber = info.GetInt32("CallerLineNumber");
                _CallerMemberName = info.GetString("CallerMemberName");
            }
        }

        public string Description
        {
            get { return _Description; }
        }

        public string CallerFilePath
        {
            get { return _CallerFilePath; }
        }

        public int CallerLineNumber
        {
            get { return _CallerLineNumber; }
        }

        public string CallerMemberName
        {
            get { return _CallerMemberName; }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Description", _Description);
            info.AddValue("CallerFilePath", _CallerFilePath);
            info.AddValue("CallerLineNumber", _CallerLineNumber);
            info.AddValue("CallerMemberName", _CallerMemberName);
        }

        public override string ToString()
        {
            var _s = typeof(MustAssertException).FullName + ": ";

            _s += Message;

            if (_CallerFilePath != null)
            {
                _s += Environment.NewLine;

                _s += "Location: " + _CallerFilePath;

                if (_CallerLineNumber > 0)
                {
                    _s += ":" + _CallerLineNumber;
                }
            }

            if (_CallerMemberName != null)
            {
                _s += Environment.NewLine;
                _s += "Member: " + _CallerMemberName;
            }

            return _s;
        }
    }
}
