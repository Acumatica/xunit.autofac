using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Xunit.Autofac.Usage.Tests
{
    public class SimpleTests
    {
        public SimpleTests(ITestOutputHelper testOutputHelper, ITestCase testCase)
        {
            testOutputHelper.WriteLine($"{testCase.GetType().Name}: {testCase.DisplayName}");
        }

        [Fact]
        public void Fact()
        {
            
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void InlineData(int no)
        {
            
        }

        [Theory]
        [MemberData(nameof(Provider))]
        public void MemberData(int no)
        {
            
        }

        public static IEnumerable<object[]> Provider()
        {
            yield return new object[]{1};
            yield return new object[]{2};
        }

        [Theory]
        [MemberData(nameof(Provider), DisableDiscoveryEnumeration = true)]
        public void MemberDataNoEnumeration(int no)
        {

        }
    }
}
