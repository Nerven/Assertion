using System.Collections.Generic;
using System.Linq;
using Nerven.Assertion.Helpers;

namespace Nerven.Assertion
{
    public sealed class MustAssertionRecord
    {
        public MustAssertionRecord(
            IReadOnlyList<MustAssertionData> data, 
            string description, 
            string callerFilePath, 
            int? callerLineNumber, 
            string callerMemberName, 
            string assemblyFullName, 
            string assemblyVersion)
        {
            Data = ReadOnlyCollectionHelper.AsReadOnly(data?.ToList());
            Description = description;
            CallerFilePath = callerFilePath;
            CallerLineNumber = callerLineNumber;
            CallerMemberName = callerMemberName;
            AssemblyFullName = assemblyFullName;
            AssemblyVersion = assemblyVersion;
        }

        public IReadOnlyList<MustAssertionData> Data { get; }

        public string Description { get; }

        public string CallerFilePath { get; }

        public int? CallerLineNumber { get; }

        public string CallerMemberName { get; }

        public string AssemblyFullName { get; }

        public string AssemblyVersion { get; }
    }
}
