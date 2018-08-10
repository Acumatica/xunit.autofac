## About the project

This project contains an integration between [xUnit](https://github.com/xunit/xunit) and [Autofac](https://github.com/autofac/Autofac).
It adds the support for the dependency injection (DI) into all xUnit entities such as test classes, fixtures, etc.

### How to use

Install the [NuGet package](https://www.nuget.org/packages/xunit.autofac).

    Install-Package xunit.autofac

In your testing project, add the following attribute to the `AssemblyInfo.cs` file.

```cs
[assembly: Xunit.Autofac.UseAutofacTestFramework]
```

Register your dependencies by adding Autofac modules to your testing project.

```cs
public class ServiceRegistration : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.Register(context => new MyService())
			.As<IMyService>()
			.InstancePerLifetimeScope();
	}
}
```

After that, you can use dependency injection in the test entities in your code.

```cs
public class TestClass
{
	private readonly IMyService _myService;

	public TestClass(IMyService myService)
	{
		_myService = myService; // this dependency will be injected automatically by Autofac
	}

	[Fact]
	public void Test()
	{
	}
}
```

## License
[MIT](https://github.com/Acumatica/xunit.autofac/blob/master/LICENSE)
