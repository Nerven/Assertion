using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace Nerven.Assertion.Tests
{
    [Collection("ExclusiveAccess_MustAssertionApi.ReportSource")]
    public sealed class UseCases
    {
        [Fact]
        public void Demo1()
        {
            Must.Assertion
                .Assert(int.Parse("1") == 1)
                .Assume(() => int.Parse("2") == 1);
        }

        [Fact]
        public void Demo2()
        {
            var _reportNumber = 0;
            MustAssertionApi.ReportSource.ForEachAsync(_report =>
                {
                    _reportNumber++;
                });

            var _exception = Assert.Throws<MustAssertionException>(() =>
                Must.Assertion
                    .Assert(int.Parse("2") == 1));
            var _memberNameFilePathLineNumber = _GetCallerMemberNameAndFilePathAndLineNumber(-1);

            Assert.Equal(0, _exception.AssertionRecord?.Data?.Count);
            Assert.Equal(nameof(Demo2), _exception.AssertionRecord?.CallerMemberName);
            Assert.Equal(_memberNameFilePathLineNumber.Item1, _exception.AssertionRecord?.CallerMemberName);
            Assert.Equal(_memberNameFilePathLineNumber.Item2, _exception.AssertionRecord?.CallerFilePath);
            Assert.Equal(_memberNameFilePathLineNumber.Item3, _exception.AssertionRecord?.CallerLineNumber);

            Assert.Equal(1, _reportNumber);
        }

        [Fact]
        public void Demo3()
        {
            var _exception = Assert.Throws<MustAssertionException>(() =>
                Must.Assertion
                    .UsingData("X", () => 1)
                    ////.WithData(_add => _add("X", typeof(int), 4))
                    .Assert(int.Parse("1") == 1)
                    .Assert(int.Parse("2") == 1)
                    .Assert(int.Parse("3") == 3));
            var _memberNameFilePathLineNumber = _GetCallerMemberNameAndFilePathAndLineNumber(-2);

            Assert.Equal(1, _exception.AssertionRecord?.Data?.Count);
            var _dataItem = _exception.AssertionRecord?.Data?.SingleOrDefault();
            Assert.Equal("X", _dataItem?.Key);
            Assert.Equal(1, _dataItem?.Value);
            Assert.Equal(nameof(Demo3), _exception.AssertionRecord?.CallerMemberName);
            Assert.Equal(_memberNameFilePathLineNumber.Item1, _exception.AssertionRecord?.CallerMemberName);
            Assert.Equal(_memberNameFilePathLineNumber.Item2, _exception.AssertionRecord?.CallerFilePath);
            Assert.Equal(_memberNameFilePathLineNumber.Item3, _exception.AssertionRecord?.CallerLineNumber);
            Assert.Equal(Assembly.GetExecutingAssembly().FullName, _exception.AssertionRecord?.AssemblyFullName);
            Version _assemblyVersion;
            Assert.True(Version.TryParse(_exception.AssertionRecord?.AssemblyVersion, out _assemblyVersion));
        }

        [Fact]
        public void Demo4()
        {
            // ReSharper disable once InconsistentNaming
            const bool Data0 = false;
            // ReSharper disable once InconsistentNaming
            const string Data1 = "1";
            // ReSharper disable once InconsistentNaming
            const int Data2 = 2;

            Must.Assertion
                .UsingData(() => new
                {
                    Data0,
                    Data1,
                    Data2
                })
                .Assert(int.Parse("1") == 1)
                .Assert(int.Parse("2") == 2)
                .Assert(int.Parse("3") == 3);
        }

        [Fact]
        public void Demo5()
        {
            var _exception = Assert.Throws<MustAssertionException>(() =>
                Must.Assertion
                    .UsingData(() => MustAssertionData.Create("A", 4))
                    .UsingData("B", () => 4)
                    .UsingData(() => new[] { MustAssertionData.Create("C", 4), MustAssertionData.Create("D", 4) })
                    .Assert(int.Parse("1") == 1)
                    .Assert(int.Parse("2") == 1)
                    .Assert(int.Parse("3") == 3));

            Assert.Equal(4, _exception.AssertionRecord?.Data?.Count);
        }

        [Fact]
        public void Demo6()
        {
            var _reportNumber = 0;
            MustAssertionApi.ReportSource.ForEachAsync(_report =>
                {
                    switch (_reportNumber)
                    {
                        case 0:
                            Assert.Equal(MustAssertionType.Assume, _report.AssertionType);
                            Assert.Equal(null, _report.AssertionRecord.Description);
                            Assert.Equal(4, _report.AssertionRecord.Data?.Count);
                            break;
                        case 1:
                            Assert.Equal(MustAssertionType.Assume, _report.AssertionType);
                            Assert.Equal("Assumption failed sadly.", _report.AssertionRecord.Description);
                            Assert.Equal(6, _report.AssertionRecord.Data?.Count);
                            break;
                        case 2:
                            Assert.Equal(MustAssertionType.Assert, _report.AssertionType);
                            Assert.Equal(null, _report.AssertionRecord.Description);
                            Assert.Equal(7, _report.AssertionRecord.Data?.Count);
                            break;
                        default:
                            Assert.False(true);
                            break;
                    }

                    _reportNumber++;
                });

            var _exception = Assert.Throws<MustAssertionException>(() =>
                Must.Assertion
                    .UsingData(() => MustAssertionData.Create("A", 4))
                    .UsingData("B", () => 4)
                    .UsingData(() => new[] { MustAssertionData.Create("C", 4), MustAssertionData.Create("D", 4) })
                    .Assume(() => int.Parse("1") == 1)
                    .Assume(() => int.Parse("1") == 2)
                    .UsingData("C", () => 5)
                    .Assert(int.Parse("1") == 1)
                    .UsingData("D", () => 6)
                    .Assume(() => int.Parse("1") == 3, "Assumption failed sadly.")
                    .UsingData("E", () => 7)
                    .Assert(int.Parse("2") == 1)
                    .Assume(() => int.Parse("1") == 1)
                    .UsingData("F", () => 8)
                    .Assert(int.Parse("3") == 3));

            Assert.Equal(7, _exception.AssertionRecord?.Data?.Count);
            Assert.Equal(3, _reportNumber);
        }

        private static Tuple<string, string, int> _GetCallerMemberNameAndFilePathAndLineNumber(
            int lineNumberOffset,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            return Tuple.Create(callerMemberName, callerFilePath, callerLineNumber + lineNumberOffset);
        }
    }
}
