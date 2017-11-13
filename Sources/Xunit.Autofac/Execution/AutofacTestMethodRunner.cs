using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac.Execution
{
    internal class AutofacTestMethodRunner: XunitTestMethodRunner
    {
        private readonly IMessageSink _diagnosticMessageSink;
        private readonly object[] _constructorArguments;
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacTestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, object[] constructorArguments, ILifetimeScope lifetimeScope) : base(testMethod, @class, method, testCases, diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource, constructorArguments)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
            _constructorArguments = constructorArguments;
            _lifetimeScope = lifetimeScope;
        }

        protected override async Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope(cb =>
            {
                cb
                    .RegisterInstance(testCase)
                    .As<ITestCase>()
                    .ExternallyOwned();
            }))
            {
                object[] constructorArguments = null;
                Aggregator.Run(() => constructorArguments = ResolveConstructorArguments(scope));
                return await testCase.RunAsync(_diagnosticMessageSink, MessageBus, constructorArguments, new ExceptionAggregator(Aggregator), CancellationTokenSource);
            }
        }

        private object[] ResolveConstructorArguments(ILifetimeScope scope)
        {
            if (_constructorArguments == null || _constructorArguments.Length == 0)
                return _constructorArguments;

            var result = new object[_constructorArguments.Length];
            for (var i = 0; i < _constructorArguments.Length; i++)
            {
                var original = _constructorArguments[i];
                var func = original as Func<ILifetimeScope, object>;
                result[i] = func != null ? func(scope) : original;
            }
            return result;
        }
    }
}