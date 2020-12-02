using Autofac;
using Xunit.Abstractions;

namespace Xunit.Autofac.Usage.TestOutputHelper.Tests
{
    public class InjectedTestOutputHelperTests
    {
	    private readonly ITestOutputHelper _testOutputHelper;

	    public InjectedTestOutputHelperTests(ITestOutputHelper testOutputHelper)
        {
	        _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Fact()
        {
	        Assert.IsType<CustomTestOutputHelper>(_testOutputHelper);
        }
    }

    public class CustomTestOutputHelper : ITestOutputHelper
    {
	    public void WriteLine(string message) { }

	    public void WriteLine(string format, params object[] args) { }
    }

    // ReSharper disable once UnusedMember.Global
    public class ServiceRegistration : Module
    {
	    protected override void Load(ContainerBuilder builder)
	    {
		    builder
			    .RegisterType<CustomTestOutputHelper>()
			    .As<ITestOutputHelper>()
			    .InstancePerLifetimeScope();
	    }
    }
}
