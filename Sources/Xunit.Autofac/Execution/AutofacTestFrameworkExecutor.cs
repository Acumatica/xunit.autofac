using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.AttributeFilters;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac.Execution
{
    internal class AutofacTestFrameworkExecutor: XunitTestFrameworkExecutor
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacTestFrameworkExecutor(AssemblyName assemblyName, ISourceInformationProvider sourceInformationProvider, IMessageSink diagnosticMessageSink, ILifetimeScope lifetimeScope)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
        {
            _lifetimeScope = lifetimeScope;
        }

        protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            using (var testAssemblyRunner = _lifetimeScope.Resolve<TestAssemblyRunner<IXunitTestCase>>(
                TypedParameter.From<ITestAssembly>(TestAssembly),
                TypedParameter.From(testCases),
                TypedParameter.From(executionOptions),
                new ResolvedParameter(
                    (pi, cc) => pi.ParameterType == typeof(IMessageSink) && (pi.GetCustomAttribute<KeyFilterAttribute>()?.Key as string) == Keys.ExecutionMessageSink,
                    (pi, cc) => executionMessageSink
                )
            ))
            {
                await testAssemblyRunner.RunAsync();
            }
        }
    }
}
