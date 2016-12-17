using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nerven.Assertion.Helpers;

namespace Nerven.Assertion
{
    public sealed class MustAssertionApi
    {
        private static readonly _ReportObservable _ReportSource = new _ReportObservable();

        private readonly Func<IEnumerable<ResolveMustAssertionDatas>> _GetDataResolvers;

        private static bool _EvaluateAssumptions = new MustAssertionOptions().EvaluateAssumptions;
        private static bool _ThrowOnFailedAssumptions = new MustAssertionOptions().ThrowOnFailedAssumptions;

        [PublicAPI]
        public static event Action<MustAssertionReport> NewReport;

        internal MustAssertionApi(Func<IEnumerable<ResolveMustAssertionDatas>> getDataResolvers)
        {
            _GetDataResolvers = getDataResolvers;
        }

        [PublicAPI]
        public static IObservable<MustAssertionReport> ReportSource => _ReportSource;

        [PublicAPI]
        public bool EvaluateAssumptions => _EvaluateAssumptions;

        [PublicAPI]
        public static void Configure(MustAssertionOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (!options.EvaluateAssumptions && options.ThrowOnFailedAssumptions)
            {
                throw new ArgumentException($"{nameof(options.ThrowOnFailedAssumptions)} cannot be {true} when {nameof(options.EvaluateAssumptions)} is {false}.", nameof(options));
            }

            _EvaluateAssumptions = options.EvaluateAssumptions;
            _ThrowOnFailedAssumptions = options.ThrowOnFailedAssumptions;
        }

        [PublicAPI]
        public MustAssertionApi UsingData(ResolveMustAssertionData resolveData)
        {
            return new MustAssertionApi(() => _YieldDatas(resolveData));
        }

        [PublicAPI]
        public MustAssertionApi UsingData(ResolveMustAssertionDatas resolveDatas)
        {
            return new MustAssertionApi(() => _YieldDatas(resolveDatas));
        }

        [PublicAPI]
        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        public void AssertionFailed(
            [CanBeNull] string description,
            [CanBeNull] TransformExceptionDelegate transformException,
            [CanBeNull] Assembly callerAssembly,
            [CanBeNull] string callerFilePath,
            [CanBeNull] int? callerLineNumber,
            [CanBeNull] string callerMemberName)
        {
            Exception[] _resolveDataErrorExceptions;
            var _data = _TryResolveData(_GetDataResolvers, out _resolveDataErrorExceptions);
            var _record = AssertionRecordHelper.CreateRecord(
                _data,
                description,
                callerAssembly,
                callerFilePath,
                callerLineNumber,
                callerMemberName);

            Exception _reportErrorException;
            _ReportFailure(MustAssertionType.Assert, _record, out _reportErrorException);

            throw AssertionExceptionHelper.CreateAssertionException(
                transformException: transformException,
                assertionRecord: _record,
                additionalExceptions: ExceptionHelper.Combine(_resolveDataErrorExceptions, ExceptionHelper.Yield(_reportErrorException)));
        }

        [PublicAPI]
        [ContractAnnotation("=> halt")]
        [DebuggerHidden]
        public void NeverAssertionFailed(
            [CanBeNull] string description,
            [CanBeNull] TransformExceptionDelegate transformException,
            [CanBeNull] Assembly callerAssembly,
            [CanBeNull] string callerFilePath,
            [CanBeNull] int? callerLineNumber,
            [CanBeNull] string callerMemberName)
        {
            Exception[] _resolveDataErrorExceptions;
            var _data = _TryResolveData(_GetDataResolvers, out _resolveDataErrorExceptions);
            var _record = AssertionRecordHelper.CreateRecord(
                _data,
                description,
                callerAssembly,
                callerFilePath,
                callerLineNumber,
                callerMemberName);

            Exception _reportErrorException;
            _ReportFailure(MustAssertionType.AssertNever, _record, out _reportErrorException);

            throw AssertionExceptionHelper.CreateAssertionException(
                transformException: transformException,
                assertionRecord: _record,
                additionalExceptions: ExceptionHelper.Combine(_resolveDataErrorExceptions, ExceptionHelper.Yield(_reportErrorException)));
        }

        [PublicAPI]
        [DebuggerHidden]
        public void AssumptionFailed(
            [CanBeNull] string description,
            [CanBeNull] Assembly callerAssembly,
            [CanBeNull] string callerFilePath,
            [CanBeNull] int? callerLineNumber,
            [CanBeNull] string callerMemberName)
        {
            Exception[] _resolveDataErrorExceptions;
            var _data = _TryResolveData(_GetDataResolvers, out _resolveDataErrorExceptions);
            var _record = AssertionRecordHelper.CreateRecord(
                _data,
                description,
                callerAssembly,
                callerFilePath,
                callerLineNumber,
                callerMemberName);

            Exception _reportErrorException;
            _ReportFailure(MustAssertionType.Assume, _record, out _reportErrorException);

            var _exception = ExceptionHelper.Combine(_resolveDataErrorExceptions, ExceptionHelper.Yield(_reportErrorException));

            if (_ThrowOnFailedAssumptions)
            {
                throw AssertionExceptionHelper.CreateAssertionException(
                    transformException: null,
                    assertionRecord: _record,
                    additionalExceptions: _exception);
            }

            if (_exception != null)
            {
                throw _exception;
            }
        }

        [PublicAPI]
        [DebuggerHidden]
        public void NeverAssumptionFailed(
            [CanBeNull] string description,
            [CanBeNull] Assembly callerAssembly,
            [CanBeNull] string callerFilePath,
            [CanBeNull] int? callerLineNumber,
            [CanBeNull] string callerMemberName)
        {
            Exception[] _resolveDataErrorExceptions;
            var _data = _TryResolveData(_GetDataResolvers, out _resolveDataErrorExceptions);
            var _record = AssertionRecordHelper.CreateRecord(
                _data,
                description,
                callerAssembly,
                callerFilePath,
                callerLineNumber,
                callerMemberName);

            Exception _reportErrorException;
            _ReportFailure(MustAssertionType.AssumeNever, _record, out _reportErrorException);

            var _exception = ExceptionHelper.Combine(_resolveDataErrorExceptions, ExceptionHelper.Yield(_reportErrorException));

            if (_ThrowOnFailedAssumptions)
            {
                throw AssertionExceptionHelper.CreateAssertionException(
                    transformException: null,
                    assertionRecord: _record,
                    additionalExceptions: _exception);
            }

            if (_exception != null)
            {
                throw _exception;
            }
        }

        [PublicAPI]
        [DebuggerHidden]
        public static void ReportCustomFailure([NotNull] MustAssertionRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            Exception _reportErrorException;
            _ReportFailure(MustAssertionType.Custom, record, out _reportErrorException);

            if (_reportErrorException != null)
            {
                throw _reportErrorException;
            }
        }

        private IEnumerable<ResolveMustAssertionDatas> _YieldDatas(ResolveMustAssertionData resolveData)
        {
            return _YieldDatas(_GetDataResolvers, () => _YieldData(resolveData));
        }

        private IEnumerable<ResolveMustAssertionDatas> _YieldDatas(ResolveMustAssertionDatas resolveDatas)
        {
            return _YieldDatas(_GetDataResolvers, resolveDatas);
        }

        private static IEnumerable<MustAssertionData> _YieldData(ResolveMustAssertionData resolveData)
        {
            yield return resolveData();
        }

        private static IEnumerable<ResolveMustAssertionDatas> _YieldDatas(Func<IEnumerable<ResolveMustAssertionDatas>> getDataResolvers, ResolveMustAssertionDatas resolveDatas)
        {
            if (getDataResolvers != null)
            {
                foreach (var _dataResolver in getDataResolvers())
                {
                    yield return _dataResolver;
                }
            }

            if (resolveDatas != null)
            {
                yield return resolveDatas;
            }
        }

        [CanBeNull]
        private static IReadOnlyList<MustAssertionData> _TryResolveData([CanBeNull] Func<IEnumerable<ResolveMustAssertionDatas>> getDataResolvers, [CanBeNull] out Exception[] exceptions)
        {
            if (getDataResolvers != null)
            {
                var _resolvers = getDataResolvers();
                var _datas = new List<MustAssertionData>();
                List<Exception> _exceptions = null;
                foreach (var _resolveDatas in _resolvers)
                {
                    IReadOnlyCollection<MustAssertionData> _resolvedDatas;

                    try
                    {
                        _resolvedDatas = _resolveDatas()?.ToList();
                    }
                    catch (Exception _exception)
                    {
                        if (_exceptions == null)
                        {
                            _exceptions = new List<Exception>();
                        }

                        _exceptions.Add(_exception);
                        continue;
                    }

                    if (_resolvedDatas != null)
                    {
                        _datas.AddRange(_resolvedDatas);
                    }
                }

                if (_exceptions != null)
                {
                    exceptions = new Exception[_exceptions.Count];
                    _exceptions.CopyTo(exceptions);
                }
                else
                {
                    exceptions = null;
                }

                return _datas.AsReadOnly();
            }

            exceptions = null;
            return new List<MustAssertionData>(0).AsReadOnly();
        }

        private static void _ReportFailure(MustAssertionType assertionType, MustAssertionRecord record, out Exception reportErrorException)
        {
            try
            {
                var _timestamp = DateTimeOffset.Now;
                _ReportSource.Push(new MustAssertionReport(_timestamp, assertionType, record));
                reportErrorException = null;
            }
            catch (Exception _exception)
            {
                reportErrorException = _exception;
            }
        }

        private sealed class _ReportObservable : IObservable<MustAssertionReport>
        {
            private readonly ConcurrentDictionary<int, IObserver<MustAssertionReport>> _Observers;
            private readonly object _IdLock = new object();
            private int _Id;

            public _ReportObservable()
            {
                _Observers = new ConcurrentDictionary<int, IObserver<MustAssertionReport>>();
            }

            private int _GetId()
            {
                lock (_IdLock)
                {
                    return _Id++;
                }
            }

            public IDisposable Subscribe(IObserver<MustAssertionReport> observer)
            {
                if (observer == null)
                {
                    throw new ArgumentNullException(nameof(observer));
                }

                var _id = _GetId();
                _Observers.TryAdd(_id, observer);
                return new _ReportUnsubscribe(_id, _Unsubscribe);
            }

            public void Push(MustAssertionReport report)
            {
                var _observers = _Observers.ToArray();
                var _observerTasks = new Task[_observers.Length];

                for (var _i = 0; _i < _observers.Length; _i++)
                {
                    var _observer = _observers[_i].Value;
                    _observerTasks[_i] = Task.Run(() => _observer.OnNext(report));
                }

                Task.WaitAll(_observerTasks);

                NewReport?.Invoke(report);
            }

            private void _Unsubscribe(int observerId)
            {
                IObserver<MustAssertionReport> _observer;
                _Observers.TryRemove(observerId, out _observer);
            }
        }

        private sealed class _ReportUnsubscribe : IDisposable
        {
            private readonly Action<int> _Unsubscribe;
            private readonly ConcurrentQueue<int> _ObserverContainer;

            public _ReportUnsubscribe(int observerId, Action<int> unsubscribe)
            {
                _ObserverContainer = new ConcurrentQueue<int>();
                _ObserverContainer.Enqueue(observerId);
                _Unsubscribe = unsubscribe;
            }

            public void Dispose()
            {
                int _observerId;
                if (_ObserverContainer.TryDequeue(out _observerId))
                {
                    _Unsubscribe(_observerId);
                }
            }
        }
    }
}
