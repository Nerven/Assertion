using System;
using System.Linq;
using Nerven.Assertion.Extensions;
using Xunit;

//// ReSharper disable MemberCanBePrivate.Global
namespace Nerven.Assertion.Tests
{
    public sealed class UseCases
    {
        public static Uri[] ConvertAbsoluteUrisToRelativeBasic(Uri baseUri, params Uri[] absoluteUris)
        {
            Must.Assertion
                .Assert(baseUri != null)
                .Assert(baseUri.IsAbsoluteUri)
                .Assert(absoluteUris != null);

            return absoluteUris.Select(_absoluteUri =>
            {
                Must.Assertion
                    .Assert(_absoluteUri != null)
                    .Assert(_absoluteUri.IsAbsoluteUri)
                    .Assert(baseUri.IsBaseOf(_absoluteUri));

                return new Uri(
                    _absoluteUri.AbsoluteUri.Substring(baseUri.AbsoluteUri.Length),
                    UriKind.Relative);
            }).ToArray();
        }

        public static Uri[] ConvertAbsoluteUrisToRelativePerfect(Uri baseUri, params Uri[] absoluteUris)
        {
            Must.Assertion
                .UsingData(nameof(baseUri), () => baseUri)
                .UsingData(nameof(absoluteUris), () => absoluteUris)
                .AssertArgumentNotNull(baseUri, nameof(baseUri))
                .Assert<ArgumentException>(baseUri.IsAbsoluteUri, $"{nameof(baseUri)} is an absolute URI")
                .AssertArgumentNotNull(absoluteUris, nameof(absoluteUris));

            return absoluteUris.Select(_absoluteUri =>
            {
                Must.Assertion
                    .UsingData(nameof(baseUri), () => baseUri)
                    .UsingData(nameof(absoluteUris), () => absoluteUris)
                    .Assert(_absoluteUri != null, $"{nameof(absoluteUris)} contains no null values")
                    .Assert(_absoluteUri.IsAbsoluteUri, $"{nameof(absoluteUris)} contains only absolute URIs")
                    .Assert(baseUri.IsBaseOf(_absoluteUri), $"{nameof(absoluteUris)} contains only URIs with {nameof(baseUri)} as base");

                return new Uri(
                    _absoluteUri.AbsoluteUri.Substring(baseUri.AbsoluteUri.Length),
                    UriKind.Relative);
            }).ToArray();
        }

        // "Perfect code" usage, with custom exceptions and assertion descriptions
        public static Uri[] ConvertRelativeUrisToAbsolute(Uri baseUri, params Uri[] relativeUris)
        {
            Must.Assertion
                .AssertArgumentNotNull(baseUri, nameof(baseUri))
                .AssertArgumentNotNull(relativeUris, nameof(relativeUris))
                .Assert<ArgumentException>(baseUri.IsAbsoluteUri, "baseUri is absolute");

            return relativeUris.Select(_relativeUri =>
            {
                Must.Assertion
                    .Assert<ArgumentException>(
                        _relativeUri != null, "relativeUris contains no null values")
                    .Assert<ArgumentException>(
                        !_relativeUri.IsAbsoluteUri, "relativeUris contains only relative URIs");

                return new Uri(baseUri, _relativeUri);
            }).ToArray();
        }

        [Fact]
        public void ConvertAbsoluteUrisToRelativeBasicCase1()
        {
            var _exception = Assert.Throws<MustAssertionException>(() => ConvertAbsoluteUrisToRelativeBasic(
                new Uri("http://example.com"),
                new Uri("http://example.com/otto"),
                new Uri("https://example.com/otto"),
                new Uri("http://example.com/per/otto")));

            Assert.Contains("Assertion failed", _exception.Message);
            Assert.Equal("ConvertAbsoluteUrisToRelativeBasic", _exception.AssertionRecord.CallerMemberName);
        }

        [Fact]
        public void ConvertAbsoluteUrisToRelativePerfectCase1()
        {
            var _exception = Assert.Throws<MustAssertionException>(() => ConvertAbsoluteUrisToRelativePerfect(
                new Uri("http://example.com"),
                new Uri("http://example.com/otto"),
                new Uri("https://example.com/otto"),
                new Uri("http://example.com/per/otto")));

            Assert.Contains("Assertion failed: Asserts that absoluteUris contains only URIs with baseUri as base", _exception.Message);
            Assert.Equal("ConvertAbsoluteUrisToRelativePerfect", _exception.AssertionRecord.CallerMemberName);
        }

        [Fact]
        public void ConvertRelativeUrisToAbsoluteCase1()
        {
            var _exception = Assert.Throws<ArgumentException>(() => ConvertRelativeUrisToAbsolute(
                new Uri("http://example.com/otto/per"),
                new Uri("/otto", UriKind.Relative),
                new Uri("./../otto", UriKind.Relative),
                new Uri("../../per/otto", UriKind.Relative),
                new Uri("https://example.com/otto")));

            Assert.Contains("Assertion failed: Asserts that relativeUris contains only relative URIs", ((MustAssertionException)_exception.InnerException).Message);
            Assert.Equal("ConvertRelativeUrisToAbsolute", ((MustAssertionException)_exception.InnerException).AssertionRecord.CallerMemberName);
        }

        [Fact]
        public void ConvertRelativeUrisToAbsoluteCase2()
        {
            ConvertRelativeUrisToAbsolute(
                new Uri("http://example.com/otto/per"),
                new Uri("/otto", UriKind.Relative),
                new Uri("./../otto", UriKind.Relative),
                new Uri("../../per/otto", UriKind.Relative),
                new Uri("../otto", UriKind.Relative));
        }
    }
}
