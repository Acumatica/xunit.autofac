using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Autofac
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [TestFrameworkDiscoverer("Xunit.Autofac." + nameof(AutofacTestFrameworkTypeDiscoverer), "Xunit.Autofac")]
    public class UseAutofacTestFrameworkAttribute: Attribute, ITestFrameworkAttribute
    {
    }

    internal class AutofacTestFrameworkTypeDiscoverer: ITestFrameworkTypeDiscoverer
    {
        public Type GetTestFrameworkType(IAttributeInfo attribute) => typeof(AutofacTestFramework);
    }
}
