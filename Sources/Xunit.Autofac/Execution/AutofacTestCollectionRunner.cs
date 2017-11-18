using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac.Execution
{
    internal class AutofacTestCollectionRunner: XunitTestCollectionRunner
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacTestCollectionRunner(ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, 
            IMessageSink diagnosticMessageSink, 
            IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource,
            ILifetimeScope lifetimeScope
            ) : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            _lifetimeScope = lifetimeScope;
        }

        protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases)
        {
            return _lifetimeScope
                .Resolve<TestClassRunner<IXunitTestCase>>(
                    TypedParameter.From(testClass),
                    TypedParameter.From(@class),
                    TypedParameter.From(testCases),
                    TypedParameter.From(TestCaseOrderer),
                    TypedParameter.From(new ExceptionAggregator(Aggregator)),
                    TypedParameter.From(CancellationTokenSource),
                    TypedParameter.From<IDictionary<Type, object>>(CollectionFixtureMappings))
                .RunAsync();
        }

	    protected override void CreateCollectionFixture(Type fixtureType)
	    {
		    this.Aggregator.Run(() => this.CollectionFixtureMappings[fixtureType] = _lifetimeScope.Resolve(fixtureType));
	    }
    }
}
