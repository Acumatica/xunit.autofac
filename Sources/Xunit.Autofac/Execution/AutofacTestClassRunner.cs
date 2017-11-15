using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac.Execution
{
    internal class AutofacTestClassRunner: XunitTestClassRunner
    {
	    private readonly IDictionary<Type, object> _collectionFixtureMappings;
	    private readonly ILifetimeScope _lifetimeScope;

        public AutofacTestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases, 
            IMessageSink diagnosticMessageSink, 
            IMessageBus messageBus, 
            ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, IDictionary<Type, object> collectionFixtureMappings,
            ILifetimeScope lifetimeScope
            ) : base(testClass, @class, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
        {
	        _collectionFixtureMappings = collectionFixtureMappings;
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

        protected override bool TryGetConstructorArgument(ConstructorInfo constructor, int index, ParameterInfo parameter, out object argumentValue)
        {
            if (base.TryGetConstructorArgument(constructor, index, parameter, out argumentValue))
                return true;

            argumentValue = (Func<ILifetimeScope, object>) (s => s.Resolve(parameter.ParameterType));
            return true;
        }

	    protected override void CreateClassFixture(Type fixtureType)
	    {
		    IEnumerable<Parameter> collectionFixturePars = _collectionFixtureMappings
			    .Select(_ => new TypedParameter(_.Key, _.Value));
		    object fixtureInstance = _lifetimeScope
			    .Resolve(fixtureType, collectionFixturePars);
		    ClassFixtureMappings[fixtureType] = fixtureInstance;
	    }
    }
}
