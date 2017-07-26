using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.AttributeFilters;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac
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
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
        {
            _lifetimeScope = lifetimeScope;
        }

        protected override async Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus, ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, CancellationTokenSource cancellationTokenSource)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope(cb =>
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
    }
}
