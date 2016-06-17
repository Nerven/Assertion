using System;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    public sealed class MustAssertionData
    {
        private MustAssertionData([CanBeNull] string key, [CanBeNull] Type type, [CanBeNull] object value)
        {
            Key = key;
            Type = type;
            Value = value;
        }

        public static MustAssertionData Create<T>(string key, T value)
        {
            return new MustAssertionData(key, typeof(T), value);
        }

        public static MustAssertionData Create(string key, Type type, object value)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (value != null && value.GetType() != type)
            {
                throw new ArgumentException();
            }

            return new MustAssertionData(key, type, value);
        }

        [PublicAPI]
        public string Key { [CanBeNull] get; }

        [PublicAPI]
        public Type Type { [CanBeNull] get;  }

        [PublicAPI]
        public object Value { [CanBeNull] get; }
    }
}
