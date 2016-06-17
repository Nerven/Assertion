using System.Diagnostics;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    public sealed class MustAssertion : IMustAssertable
    {
        internal MustAssertion(MustAssertionApi api)
        {
            Api = api;
        }

        internal static MustAssertion _DefaultInstance { get; } = new MustAssertion(new MustAssertionApi(null));

        [PublicAPI]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public MustAssertionApi Api { [NotNull] get; }
    }
}
