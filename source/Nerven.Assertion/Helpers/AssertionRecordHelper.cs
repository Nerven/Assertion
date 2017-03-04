using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Nerven.Assertion.Helpers
{
    public static class AssertionRecordHelper
    {
        public static MustAssertionRecord CreateRecord(
            [CanBeNull] IReadOnlyList<MustAssertionData> data,
            [CanBeNull] string description,
            [CanBeNull] Assembly callerAssembly,
            [CanBeNull] string callerFilePath,
            [CanBeNull] int? callerLineNumber,
            [CanBeNull] string callerMemberName)
        {
            var _assemblyFullName = callerAssembly?.FullName;
            var _assemblyVersion = callerAssembly?.GetName().Version.ToString();

            return new MustAssertionRecord(
                data,
                description,
                callerFilePath,
                callerLineNumber,
                callerMemberName,
                _assemblyFullName,
                _assemblyVersion);
        }

        public static string BuildMessage(MustAssertionRecord assertionRecord)
        {
            var _newLine = Environment.NewLine;
            var _s = assertionRecord?.Description == null ? "Assertion failed" : $"Assertion failed: Asserts that {assertionRecord.Description}";

            if (assertionRecord?.CallerFilePath != null)
            {
                _s += $"{_newLine}Location: {assertionRecord.CallerFilePath}";

                if (assertionRecord.CallerLineNumber.GetValueOrDefault() != 0)
                {
                    _s += $":{assertionRecord.CallerLineNumber}";
                }
            }

            if (assertionRecord?.CallerMemberName != null)
            {
                _s += $"{_newLine}Member: {assertionRecord.CallerMemberName}";
            }

            if (assertionRecord?.Data?.Count > 0)
            {
                foreach (var _data in assertionRecord.Data)
                {
                    string _dataString;
                    try
                    {
                        _dataString = _data.Value?.ToString() ?? "<null>";
                    }
                    catch (Exception _exception)
                    {
                        _dataString = $"<unrepresentable: {_exception.Message}>";
                    }

                    _s += $"{_newLine}{_data.Key} <{_data.Type}>: {_dataString}";
                }
            }

            return _s;
        }
    }
}
