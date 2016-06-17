using JetBrains.Annotations;

namespace Nerven.Assertion
{
    public static class Must
    {
        [PublicAPI]
        public static MustAssertion Assertion
        {
            [NotNull] get { return MustAssertion._DefaultInstance; }
        }
    }
}
