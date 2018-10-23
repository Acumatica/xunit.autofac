using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.AttributeFilters;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac.Execution
{
    internal class AutofacTestAssemblyRunner: XunitTestAssemblyRunner
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacTestAssemblyRunner(ITestAssembly testAssembly, IEnumerable<IXunitTestCase> testCases, 
            IMessageSink diagnosticMessageSink,
            [KeyFilter(Keys.ExecutionMessageSink)] IMessageSink executionMessageSink,
            ITestFrameworkExecutionOptions executionOptions,
            ILifetimeScope lifetimeScope
            )
            : base(testAssembly, testCases, new Sink(diagnosticMessageSink), new Sink(executionMessageSink), executionOptions)
        {
            _lifetimeScope = lifetimeScope;
        }

        protected override async Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus, ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, CancellationTokenSource cancellationTokenSource)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope(LifetimeScopeTags.TestCollection, cb =>
            {
                cb
                    .RegisterInstance(messageBus)
                    .ExternallyOwned();
            }))
            {
                return await scope
                    .Resolve<TestCollectionRunner<IXunitTestCase>>(
                        TypedParameter.From(testCollection),
                        TypedParameter.From(testCases),
                        TypedParameter.From(cancellationTokenSource),
                        TypedParameter.From(TestCaseOrderer),
                        TypedParameter.From(new ExceptionAggregator(Aggregator))
                    )
                    .RunAsync();
            }
        }

        class Sink : DelegatingMessageSink
        {
            public Sink(IMessageSink innerSink, Action<IMessageSinkMessage> callback = null) : base(innerSink, callback)
            {
            }

            public override bool OnMessage(IMessageSinkMessage message)
            {
                if (message is ITestStarting)
                {
                    Debug.WriteLine($"Test starting {(message as ITestStarting).TestCase.DisplayName}");
                }
                if (message is ITestCaseStarting)
                {
                    Debug.WriteLine($"Test case starting {(message as ITestCaseStarting).TestCase.DisplayName}");
                }
                if (message is ITestCaseFinished)
                {
                    Debug.WriteLine($"Test case finished {(message as ITestCaseFinished).TestCase.DisplayName}");
                }
                if (message is ITestFinished)
                {
                    Debug.WriteLine($"Test finished {(message as ITestFinished).TestCase.DisplayName}");
                }
                return base.OnMessage(message);
            }
        }
    }
}
