using JetBrains.Annotations;

namespace Nerven.Assertion
{
    [PublicAPI]
    public sealed class MustAssertionOptions
    {
        [PublicAPI]
        public bool EvaluateAssumptions { get; set; } = true;

        [PublicAPI]
        public bool ThrowOnFailedAssumptions { get; set; }
    }
}
