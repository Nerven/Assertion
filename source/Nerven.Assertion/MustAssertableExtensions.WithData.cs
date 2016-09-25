using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Nerven.Assertion
{
    partial class MustAssertableExtensions
    {
        [PublicAPI]
        [AssertionMethod]
        [DebuggerHidden]
        public static IMustAssertable UsingData(
            this IMustAssertable must,
            ResolveMustAssertionData resolveData)
        {
            var _api = must.Api.UsingData(() => new[] { resolveData() });
            return new MustAssertion(_api);
        }

        [PublicAPI]
        [AssertionMethod]
        [DebuggerHidden]
        public static IMustAssertable UsingData(
            this IMustAssertable must,
            ResolveMustAssertionDatas resolveDatas)
        {
            var _api = must.Api.UsingData(resolveDatas);
            return new MustAssertion(_api);
        }

        [PublicAPI]
        [AssertionMethod]
        [DebuggerHidden]
        public static IMustAssertable UsingData<T>(
            this IMustAssertable must,
            string name,
            Func<T> getValue)
        {
            return UsingData(must, () => MustAssertionData.Create(name, getValue()));
        }

        [PublicAPI]
        [AssertionMethod]
        [DebuggerHidden]
        public static IMustAssertable UsingData(
            this IMustAssertable must,
            Func<object> getDataContainer)
        {
            return UsingData(must, () => _GetDataFromContainer(getDataContainer()));
        }

        private static IEnumerable<MustAssertionData> _GetDataFromContainer(object dataContainer)
        {
            return dataContainer == null
                ? Enumerable.Empty<MustAssertionData>()
                : dataContainer
                    .GetType()
                    .GetRuntimeProperties()
                    .Select(_property => MustAssertionData.Create(_property.Name, _property.PropertyType, _property.GetValue(dataContainer)));
        }
    }
}
