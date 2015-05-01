using System;
using System.Collections.Generic;
using Xunit;

namespace Nerven.Assertion.Tests
{
    public sealed class MustAssertTests
    {
        public static IEnumerable<object[]> GetReturnedInstanceAlwaysSameAsInstanceTheoryData()
        {
            yield return new object[] { MustAssert.Instance };
            yield return new object[] { Must.Assert(true) };
            yield return new object[] { Must.Assert(() => true) };
            yield return new object[] { Must.Assert<InvalidOperationException>(true) };
            yield return new object[] { Must.Assert<ArgumentOutOfRangeException>(() => true) };
            yield return new object[] { Must.Assert<InvalidOperationException>(true).Assert(true) };
            yield return new object[] { Must.Assert(true).Assert<ArgumentOutOfRangeException>(() => true).Assert<InvalidOperationException>(true) };
            yield return new object[] { Must.Assert(true).Assert<ArgumentOutOfRangeException>(() => true).Assert<InvalidOperationException>(true).Assert(true) };
        }

        [Fact]
        public void InstanceNeverNull()
        {
            Assert.NotNull(MustAssert.Instance);
        }

        [Fact]
        public void InstanceAlwaysSame()
        {
            Assert.Same(MustAssert.Instance, MustAssert.Instance);
        }

        [Theory]
        [MemberData("GetReturnedInstanceAlwaysSameAsInstanceTheoryData")]
        public void ReturnedInstanceAlwaysSameAsInstance(MustAssert assert)
        {
            Assert.Same(MustAssert.Instance, assert);
        }

        [Fact]
        public void TrueAssertionThrowsNoException()
        {
            MustAssert.Instance.Assert(true);
        }

        [Fact]
        public void TrueAssertionActionThrowsNoException()
        {
            MustAssert.Instance.Assert(() => true);
        }

        [Fact]
        public void FalseAssertionThrowsRightException()
        {
            Assert.Throws<MustAssertException>(() => MustAssert.Instance.Assert(false));
        }

        [Fact]
        public void FalseAssertionWithDescriptionThrowsWithDescription()
        {
            try
            {
                MustAssert.Instance.Assert(false, "Description test");
            }
            catch (Exception _exception)
            {
                var _assertException = (MustAssertException)_exception;

                Assert.Equal(_assertException.Description, "Description test");
                return;
            }

            //// ReSharper disable once HeuristicUnreachableCode
            Assert.True(false);
        }

        [Fact]
        public void FalseAssertionActionThrowsRightException()
        {
            Assert.Throws<MustAssertException>(() => MustAssert.Instance.Assert(() => false));
        }

        [Fact]
        public void TrueAssertionWithCustomExceptionTypeParameterThrowsNoException()
        {
            MustAssert.Instance.Assert<ArgumentException>(true);
        }

        [Fact]
        public void TrueAssertionWithCustomExceptionTypeParameterActionThrowsNoException()
        {
            MustAssert.Instance.Assert<ArgumentException>(() => true);
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionTypeParameterThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => MustAssert.Instance.Assert<ArgumentException>(false));
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionTypeParameterActionThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => MustAssert.Instance.Assert<ArgumentException>(() => false));
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionTypeParameterAndDescriptionThrowsWithDescription()
        {
            try
            {
                //// ReSharper disable once NotResolvedInText
                MustAssert.Instance.Assert<ArgumentException>(false, "Description test");
            }
            catch (ArgumentException _thrownException)
            {
                Exception _exception = _thrownException;
                while (_exception.InnerException != null)
                {
                    _exception = _exception.InnerException;
                }

                var _assertException = (MustAssertException)_exception;

                Assert.Equal(_assertException.Description, "Description test");
                return;
            }

            //// ReSharper disable once HeuristicUnreachableCode
            Assert.True(false);
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionThrowsRightException()
        {
            //// ReSharper disable once NotResolvedInText
            Assert.Throws<ArgumentException>(() => MustAssert.Instance.Assert(false, _innerException => new ArgumentException("Message", "Parameter", _innerException)));
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionAndDescriptionThrowsWithDescription()
        {
            try
            {
                //// ReSharper disable once NotResolvedInText
                MustAssert.Instance.Assert(false, _innerException => new ArgumentException("Message", "Parameter", _innerException), "Description test");
            }
            catch (ArgumentException _thrownException)
            {
                Exception _exception = _thrownException;
                while (_exception.InnerException != null)
                {
                    _exception = _exception.InnerException;
                }

                var _assertException = (MustAssertException)_exception;

                Assert.Equal(_assertException.Description, "Description test");
                return;
            }

            //// ReSharper disable once HeuristicUnreachableCode
            Assert.True(false);
        }
    }
}
