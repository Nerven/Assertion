using System.Collections;
using System.Collections.Generic;

namespace Nerven.Assertion.Helpers
{
    public static class ReadOnlyCollectionHelper
    {
        public static IReadOnlyList<T> AsReadOnly<T>(this IReadOnlyList<T> collection)
        {
            return new _ReadOnlyList<T>(collection);
        }

        private sealed class _ReadOnlyList<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _Collection;

            public _ReadOnlyList(IReadOnlyList<T> collection)
            {
                _Collection = collection;
            }

            public int Count => _Collection.Count;

            public T this[int index] => _Collection[index];

            public IEnumerator<T> GetEnumerator() => _Collection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
