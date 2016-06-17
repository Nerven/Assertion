using System;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    [NotNull]
    public delegate Exception TransformExceptionDelegate([NotNull] MustAssertionException innerException);
}
