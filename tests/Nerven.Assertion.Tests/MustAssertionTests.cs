using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Nerven.Assertion.Tests
{
    public sealed class MustAssertionTests
    {
        [Fact]
        public void TrueAssertionThrowsNoException()
        {
            MustAssertion._DefaultInstance.Assert(true);
        }

        [Fact]
        public void FalseAssertionThrowsRightException()
        {
            Assert.Throws<MustAssertionException>(() => MustAssertion._DefaultInstance.Assert(false));
        }

        [Fact]
        public void TrueAssertionWithCustomExceptionThrowsNoException()
        {
            MustAssertion._DefaultInstance.Assert(true, _e => new ArgumentException(null, _e));
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => MustAssertion._DefaultInstance.Assert(false, _e => new ArgumentException(null, _e)));
        }

        [Fact]
        public void TrueAssertionWithCustomExceptionTypeParameterThrowsNoException()
        {
            MustAssertion._DefaultInstance.Assert<ArgumentException>(true);
        }

        [Fact]
        public void FalseAssertionWithCustomExceptionTypeParameterThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => MustAssertion._DefaultInstance.Assert<ArgumentException>(false));
        }

        [Fact]
        public void AssertionWithData()
        {
            MustAssertion._DefaultInstance
                .UsingData("ppttp", () => "x")
                .UsingData(null, () => "234s")
                .Assert(1 != int.Parse("0"));
        }

        [Fact]
        public void NeverThrows()
        {
            Assert.Throws<MustAssertionException>(() => MustAssertion._DefaultInstance.AssertNever());
        }

        [Fact]
        public void NeverWithDescriptionThrows()
        {
            Assert.Throws<MustAssertionException>(() => MustAssertion._DefaultInstance.AssertNever("X"));
        }
        
        [Fact]
        public void NeverWithDescriptionThrowsWithExpectedExceptionMessage()
        {
            var _actualException = Assert.Throws<MustAssertionException>(() => MustAssertion._DefaultInstance.AssertNever("Custom description expected in exception message."));
            Assert.Contains("Custom description expected in exception message.", _actualException.Message);
        }

        [Fact]
        public void NeverWithCustomExceptionThrowsRightException()
        {
            Assert.Throws<ArgumentException>(() => MustAssertion._DefaultInstance.AssertNever(_e => new ArgumentException(null, _e)));
        }

        [Fact]
        public void NeverWithCustomExceptionTypeParamterThrowsRightException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MustAssertion._DefaultInstance.AssertNever<ArgumentOutOfRangeException>());
        }

        [Fact]
        public void NeverWithCustomExceptionTypeParamterThrowsExceptionWithMustInnerException()
        {
            var _actualException = Assert.Throws<InvalidOperationException>(() => MustAssertion._DefaultInstance.AssertNever<InvalidOperationException>());

            Assert.IsType<MustAssertionException>(_actualException.InnerException);
        }

        [Fact]
        public void InstanceNeverNull()
        {
            Assert.NotNull(Must.Assertion);
            Assert.NotNull(MustAssertion._DefaultInstance);
        }

        [Fact]
        public void InstanceAlwaysSame()
        {
            Assert.Same(MustAssertion._DefaultInstance, MustAssertion._DefaultInstance);
            Assert.Same(MustAssertion._DefaultInstance, Must.Assertion);
            Assert.Same(Must.Assertion, Must.Assertion);
        }
        
        [Fact]
        public void FalseAssertionWithDescriptionThrowsWithDescription()
        {
            try
            {
                Must.Assertion.Assert(false, "Description test");
            }
            catch (Exception _exception)
            {
                var _assertException = (MustAssertionException)_exception;

                Assert.Equal("Description test", _assertException.AssertionRecord.Description);
                return;
            }

            //// ReSharper disable once HeuristicUnreachableCode
            Assert.True(false);
        }
        
        [Fact]
        public void FalseAssertionWithCustomExceptionTypeParameterAndDescriptionThrowsWithDescription()
        {
            try
            {
                Must.Assertion.Assert<ArgumentException>(false, "Description test");
            }
            catch (ArgumentException _thrownException)
            {
                Exception _exception = _thrownException;
                while (_exception.InnerException != null)
                {
                    _exception = _exception.InnerException;
                }

                var _assertException = (MustAssertionException)_exception;

                Assert.Equal("Description test", _assertException.AssertionRecord.Description);
                return;
            }

            //// ReSharper disable once HeuristicUnreachableCode
            Assert.True(false);
        }
        
        [Fact]
        public void FalseAssertionWithCustomExceptionAndDescriptionThrowsWithDescription()
        {
            try
            {
                //// ReSharper disable once NotResolvedInText
                Must.Assertion.Assert(false, "Description test", _innerException => new ArgumentException("Message", "Parameter", _innerException));
            }
            catch (ArgumentException _thrownException)
            {
                Exception _exception = _thrownException;
                while (_exception.InnerException != null)
                {
                    _exception = _exception.InnerException;
                }

                var _assertException = (MustAssertionException)_exception;

                Assert.Equal("Description test", _assertException.AssertionRecord.Description);
                return;
            }

            //// ReSharper disable once HeuristicUnreachableCode
            Assert.True(false);
        }
        
        [Fact(Skip = "Results in \"Could not load file or assembly 'JetBrains.Annotations.Dotnet[...]\" exception since target changed to .NET Standard.")]
        public void AssertionMethodsHasRequiredAttributesAndReSharperAnnotationsArePreserved()
        {
            var _assertionMethods = new[] { typeof(MustAssertableExtensions), typeof(Extensions.MustAssertableExtensions) }
                .SelectMany(_type => _type.GetMethods().Where(_method => _method.DeclaringType == _type))
                .Where(_method => _method.IsPublic)
                .ToArray();

            Assert.True(_assertionMethods.Length > 0);

            foreach (var _assertionMethod in _assertionMethods)
            {
                var _requiredAttributes = _GetRequiredAttributes(_assertionMethod);
                var _assertMethodAttributes = _assertionMethod.GetCustomAttributes(false);
                var _fullName = _assertionMethod.DeclaringType?.FullName;
                var _currentAttributes = string.Join(", ", _assertMethodAttributes.Select(_attribute => _attribute.GetType().Name));

                foreach (var _requiredAttribute in _requiredAttributes)
                {
                    var _message = $"{_fullName}.{_assertionMethod.Name} should have attribute {_requiredAttribute} (has {_currentAttributes})";
                    Assert.True(_assertMethodAttributes.Any(_assertMethodAttribute => _assertMethodAttribute.GetType().Name.Equals(_requiredAttribute, StringComparison.Ordinal)), _message);
                }
            }
        }

        private IEnumerable<string> _GetRequiredAttributes(MethodInfo assertionMethod)
        {
            yield return "AssertionMethodAttribute";
            yield return "PublicAPIAttribute";
            yield return "DebuggerHiddenAttribute";

            if (assertionMethod.Name.IndexOf("Assert", StringComparison.OrdinalIgnoreCase) != -1)
            {
                yield return "ContractAnnotationAttribute";
            }
        }
    }
}
