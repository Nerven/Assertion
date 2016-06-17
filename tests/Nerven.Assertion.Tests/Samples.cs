using System;
using System.Linq;
using System.Reactive.Linq;
using Nerven.Assertion.Extensions;
using Xunit;
// ReSharper disable InconsistentNaming

//// ReSharper disable MemberCanBePrivate.Global
namespace Nerven.Assertion.Tests
{
    [Collection("ExclusiveAccess_MustAssertionApi.ReportSource")]
    public sealed class Samples
    {
        [Fact]
        public static void QuickIntro()
        {
            var parameter = GetParameterValue();
            var isSpecialParameter = IsSpecialParameter(parameter);
            // Basic usage
            Must.Assertion
                // Throws if false
                .Assert(parameter != null)
                // Throws with message if false
                .Assert(!isSpecialParameter, "Parameter is not anything special");
            
            // More fancy stuff
            Must.Assertion
                // Adds data to log/exception, value is not evaluated unless something fails
                .UsingData(nameof(parameter), () => parameter)
                // Assumptions doesn't throw, but are logged/reported
                .Assume(() => parameter.Length > 3 && parameter.Length < 18);

            MustAssertionApi.ReportSource.ForEachAsync(report =>
                {
                    var record = report.AssertionRecord;
                    Console.WriteLine($"Type of assertion: {report.AssertionType}"); // -> Assert, Assume, AssertNever, AssumeNever
                    Console.WriteLine($"Timestamp: {report.Timestamp}");
                    Console.WriteLine($"Description: {record.Description}"); // -> e.g. Parameter is not anything special
                    Console.WriteLine($"CallerFilePath: {record.CallerFilePath}"); // -> e.g. C:\Development\ProjectX\DirY\FileZ.cs
                    Console.WriteLine($"CallerLineNumber: {record.CallerLineNumber}"); // -> e.g. 22
                    Console.WriteLine($"CallerMemberName: {record.CallerMemberName}"); // -> e.g. MethodInWhichTheAssertionWasMade
                    Console.WriteLine($"AssemblyFullName: {record.AssemblyFullName}");
                    Console.WriteLine($"AssemblyVersion: {record.AssemblyVersion}");

                    foreach (var item in record.Data)
                    {
                        Console.WriteLine($"{item.Key}<{item.Type}>: {item.Value}"); // -> e.g. parameter<System.String>: Samples
                    }
                });
        }

        private static string GetParameterValue()
        {
            return nameof(Samples);
        }

        private static bool IsSpecialParameter(string parameter)
        {
            return parameter == nameof(parameter);
        }

        public static Uri[] ConvertAbsoluteUrisToRelativeBasic(Uri baseUri, params Uri[] absoluteUris)
        {
            Must.Assertion
                .Assert(baseUri != null)
                .Assert(baseUri.IsAbsoluteUri)
                .Assert(absoluteUris != null);

            return absoluteUris.Select(absoluteUri =>
                {
                    Must.Assertion
                        .Assert(absoluteUri != null)
                        .Assert(absoluteUri.IsAbsoluteUri)
                        .Assert(baseUri.IsBaseOf(absoluteUri));

                    return new Uri(
                        absoluteUri.AbsoluteUri.Substring(baseUri.AbsoluteUri.Length),
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

            return absoluteUris.Select(absoluteUri =>
                {
                    Must.Assertion
                        .UsingData(nameof(baseUri), () => baseUri)
                        .UsingData(nameof(absoluteUris), () => absoluteUris)
                        .Assert(absoluteUri != null, $"{nameof(absoluteUris)} contains no null values")
                        .Assert(absoluteUri.IsAbsoluteUri, $"{nameof(absoluteUris)} contains only absolute URIs")
                        .Assert(baseUri.IsBaseOf(absoluteUri), $"{nameof(absoluteUris)} contains only URIs with {nameof(baseUri)} as base");

                    return new Uri(
                        absoluteUri.AbsoluteUri.Substring(baseUri.AbsoluteUri.Length),
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

            return relativeUris.Select(relativeUri =>
            {
                Must.Assertion
                    .Assert<ArgumentException>(
                        relativeUri != null, "relativeUris contains no null values")
                    .Assert<ArgumentException>(
                        !relativeUri.IsAbsoluteUri, "relativeUris contains only relative URIs");

                return new Uri(baseUri, relativeUri);
            }).ToArray();
        }

        [Fact]
        public void ConvertAbsoluteUrisToRelativeBasicCase1()
        {
            var exception = Assert.Throws<MustAssertionException>(() => ConvertAbsoluteUrisToRelativeBasic(
                new Uri("http://example.com"),
                new Uri("http://example.com/otto"),
                new Uri("https://example.com/otto"),
                new Uri("http://example.com/per/otto")));

            Assert.Contains("Assertion failed", exception.Message);
            Assert.Equal("ConvertAbsoluteUrisToRelativeBasic", exception.AssertionRecord.CallerMemberName);
        }

        [Fact]
        public void ConvertAbsoluteUrisToRelativePerfectCase1()
        {
            var exception = Assert.Throws<MustAssertionException>(() => ConvertAbsoluteUrisToRelativePerfect(
                new Uri("http://example.com"),
                new Uri("http://example.com/otto"),
                new Uri("https://example.com/otto"),
                new Uri("http://example.com/per/otto")));

            Assert.Contains("Assertion failed: Asserts that absoluteUris contains only URIs with baseUri as base", exception.Message);
            Assert.Equal("ConvertAbsoluteUrisToRelativePerfect", exception.AssertionRecord.CallerMemberName);
        }

        [Fact]
        public void ConvertRelativeUrisToAbsoluteCase1()
        {
            var exception = Assert.Throws<ArgumentException>(() => ConvertRelativeUrisToAbsolute(
                new Uri("http://example.com/otto/per"),
                new Uri("/otto", UriKind.Relative),
                new Uri("./../otto", UriKind.Relative),
                new Uri("../../per/otto", UriKind.Relative),
                new Uri("https://example.com/otto")));

            Assert.Contains("Assertion failed: Asserts that relativeUris contains only relative URIs", ((MustAssertionException)exception.InnerException).Message);
            Assert.Equal("ConvertRelativeUrisToAbsolute", ((MustAssertionException)exception.InnerException).AssertionRecord.CallerMemberName);
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
