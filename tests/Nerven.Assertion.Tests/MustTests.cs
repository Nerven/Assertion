using System;
using System.Linq;
using Xunit;

namespace Nerven.Assertion.Tests
{
    public sealed class MustTests
    {
        [Fact]
        public void TrueAssertionThrowsNoException()
        {
            Must.Assert(true);
        }

        [Fact]
        public void FalseAssertionThrowsRightException()
        {
            Assert.Throws<MustAssertException>(() => Must.Assert(false));
        }

        [Fact]
        public void TrueAssertionWithCustomExceptionTypeParameterThrowsNoException()
        {
            Must.Assert<ArgumentException>(true);
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionTypeParameterThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => Must.Assert<ArgumentException>(false));
        }

        [Fact]
        public void NeverThrows()
        {
            Assert.Throws<MustAssertException>(() => Must.Never());
        }

        [Fact]
        public void NeverWithDescrptionThrows()
        {
            Assert.Throws<MustAssertException>(() => Must.Never("X"));
        }

        [Fact]
        public void ReSharperAnnotationsArePreserved()
        {
            var _relevantMethods = typeof(Must)
                .GetMethods()
                .Where(_method => _method.Name.Equals("Assert", StringComparison.Ordinal))
                .Where(_method => _method.GetParameters().Any(_methodPatarameter => _methodPatarameter.Name.Equals("assertionResult", StringComparison.Ordinal)))
                .ToArray();

            Assert.True(_relevantMethods.Length > 0);

            Assert.True(_relevantMethods.All(_assertMethod => _assertMethod.GetCustomAttributes(false)
                .Any(_assertMethodAttribute => _assertMethodAttribute.GetType().Name.Equals("ContractAnnotationAttribute", StringComparison.Ordinal))));
        }
    }
}
