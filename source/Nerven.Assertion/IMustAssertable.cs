using JetBrains.Annotations;

namespace Nerven.Assertion
{
    public interface IMustAssertable
    {
        MustAssertionApi Api { [NotNull] get; }
    }
}
