using System.Collections.Generic;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    [CanBeNull]
    public delegate IEnumerable<MustAssertionData> ResolveMustAssertionDatas();
}
