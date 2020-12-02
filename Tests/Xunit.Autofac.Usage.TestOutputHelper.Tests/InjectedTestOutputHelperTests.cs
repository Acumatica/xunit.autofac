using Autofac;
using Xunit.Abstractions;

namespace Xunit.Autofac.Usage.TestOutputHelper.Tests
{
    public class InjectedTestOutputHelperTests
    {
	    private readonly CustomLogger _logger;
	    private readonly ITestOutputHelper _outputHelper;

	    public InjectedTestOutputHelperTests(CustomLogger logger, ITestOutputHelper outputHelper)
	    {
		    _logger = logger;
		    _outputHelper = outputHelper;
	    }

        [Fact]
        public void Fact()
        {
	        Assert.IsType<CustomTestOutputHelper>(_logger.OutputHelper);
			Assert.Same(_outputHelper, _logger.OutputHelper);
        }
    }

    public class CustomTestOutputHelper : Sdk.TestOutputHelper
    {
    }

    public class CustomLogger
    {
	    public ITestOutputHelper OutputHelper { get; }

	    public CustomLogger(ITestOutputHelper outputHelper)
	    {
		    OutputHelper = outputHelper;
	    }
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

		    builder
				.RegisterType<CustomLogger>()
				.AsSelf()
			    .InstancePerLifetimeScope();
	    }
    }
}
