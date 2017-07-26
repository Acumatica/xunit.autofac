using System.Reflection;
using Autofac;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac
{
    internal class AutofacTestFramework: TestFramework
    {
        private readonly IContainer _rootContainer;

        public AutofacTestFramework(IMessageSink messageSink) : base(messageSink)
        {
            var cb = new ContainerBuilder();

            cb
                .RegisterInstance(this)
                .AsSelf()
                .As<TestFramework>()
                .ExternallyOwned();
            cb
                .RegisterInstance(SourceInformationProvider)
                .ExternallyOwned();
            cb
                .RegisterInstance(messageSink)
                .ExternallyOwned();

            cb
                .RegisterType<AutofacTestFrameworkExecutor>()
                .As<ITestFrameworkExecutor>()
                .ExternallyOwned();

            cb
                .RegisterType<AutofacTestAssemblyRunner>()
                .As<TestAssemblyRunner<IXunitTestCase>>();

            cb
                .RegisterType<AutofacTestCollectionRunner>()
                .As<TestCollectionRunner<IXunitTestCase>>();

            cb
                .RegisterType<AutofacTestClassRunner>()
                .As<TestClassRunner<IXunitTestCase>>();

            cb
                .RegisterType<AutofacTestMethodRunner>()
                .As<TestMethodRunner<IXunitTestCase>>();

            cb
                .RegisterType<XunitTestFrameworkDiscoverer>()
                .As<ITestFrameworkDiscoverer>()
                .ExternallyOwned();

            _rootContainer = cb.Build();

            DisposalTracker.Add(_rootContainer);
        }

        protected override ITestFrameworkDiscoverer CreateDiscoverer(IAssemblyInfo assemblyInfo)
            => _rootContainer.Resolve<ITestFrameworkDiscoverer>(TypedParameter.From(assemblyInfo));

        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
            => _rootContainer.Resolve<ITestFrameworkExecutor>(TypedParameter.From(assemblyName));
    }
}
