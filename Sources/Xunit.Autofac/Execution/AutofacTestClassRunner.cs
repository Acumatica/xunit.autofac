using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac.Execution
{
    internal class AutofacTestClassRunner: XunitTestClassRunner
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacTestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases, 
            IMessageSink diagnosticMessageSink, 
            IMessageBus messageBus, 
            ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, IDictionary<Type, object> collectionFixtureMappings,
            ILifetimeScope lifetimeScope
            ) : base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
        {
            _lifetimeScope = lifetimeScope;
        }

        protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases, object[] constructorArguments)
        {
            return _lifetimeScope
                .Resolve<TestMethodRunner<IXunitTestCase>>(
                    TypedParameter.From(testMethod),
                    TypedParameter.From(method),
                    TypedParameter.From(testCases),
                    TypedParameter.From(constructorArguments),
                    TypedParameter.From(Class),
                    TypedParameter.From(new ExceptionAggregator(Aggregator)),
                    TypedParameter.From(CancellationTokenSource))
                .RunAsync();
        }
    }
}
