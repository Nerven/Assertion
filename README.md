A performant, easy to use, lightweight C# assertion library meant to be used both during development and in production.
Providing excellent error messages and logging/reporting capabilities.
Makes it easy to take care of all unexpected states, catching them directly when they appear, thus making them easily traceable.

Assertion targets .NET Standard 1.1 (netstandard1.1), and can thus be used in projects targeting .NET Framework 4.5 as well as projects targeting .NET Core (and lots of different portable versions).

### Quick examples

````c#
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
````

### Extended examples

````c#
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
````
