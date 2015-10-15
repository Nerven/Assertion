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
        public void TrueAssertionWithCustomExceptionThrowsNoException()
        {
            Must.Assert(true, _e => new ArgumentException(null, _e));
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => Must.Assert(false, _e => new ArgumentException(null, _e)));
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
        public void NeverWithDescriptionThrows()
        {
            Assert.Throws<MustAssertException>(() => Must.Never("X"));
        }
        
        [Fact]
        public void NeverWithDescriptionThrowsWithExcpectedExceptionMessage()
        {
            var _actualException = Assert.Throws<MustAssertException>(() => Must.Never("Custom description expected in exception message."));
            Assert.Contains("Custom description expected in exception message.", _actualException.Message);
        }

        [Fact]
        public void NeverWithCustomExceptionThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => Must.Never(_e => new ArgumentException(null, _e)));
        }

        [Fact]
        public void NeverWithCustomExceptionTypeParamterThrowsRightException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Must.Never<ArgumentOutOfRangeException>());
        }

        [Fact]
        public void NeverWithCustomExceptionTypeParamterThrowsExceptionWithMustInnerException()
        {
            var _actualException = Assert.Throws<InvalidOperationException>(() => Must.Never<InvalidOperationException>());

            Assert.IsType<MustAssertException>(_actualException.InnerException);
        }

        [Fact]
        public void ReSharperAnnotationsArePreserved()
        {
            var _relevantMethods = typeof(Must)
                .GetMethods()
                .Where(_method => _method.Name.Equals("Assert", StringComparison.Ordinal) || _method.Name.Equals("Never", StringComparison.Ordinal))
                .Where(_method => _method.GetParameters().Any(_methodPatarameter => _methodPatarameter.Name.Equals("description", StringComparison.Ordinal)))
                .ToArray();

            Assert.True(_relevantMethods.Length > 0);

            Assert.True(_relevantMethods.All(_assertMethod => _assertMethod.GetCustomAttributes(false)
                .Any(_assertMethodAttribute => _assertMethodAttribute.GetType().Name.Equals("ContractAnnotationAttribute", StringComparison.Ordinal))));
        }
    }
}
