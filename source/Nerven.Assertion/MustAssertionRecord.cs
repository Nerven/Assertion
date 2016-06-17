using System.Collections.Generic;
using System.Linq;

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
            Data = data?.ToList().AsReadOnly();
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
