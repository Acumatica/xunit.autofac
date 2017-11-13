using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Xunit.Autofac.Usage.Tests
{
    public class SimpleTests
    {
        public SimpleTests(ITestOutputHelper testOutputHelper, ITestCase testCase)
        {
            Debug.WriteLine(".ctor");
            testOutputHelper.WriteLine($"{testCase.GetType().Name}: {testCase.DisplayName}");
        }

        [Fact]
        public void Fact()
        {
            Debug.WriteLine("Fact");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void InlineData(int no)
        {
            Debug.WriteLine("InlineData");
        }

        [Theory]
        [MemberData(nameof(Provider))]
        public void MemberData(int no)
        {
            Debug.WriteLine("MemberData");
        }

        public static IEnumerable<object[]> Provider()
        {
            Debug.WriteLine("Provider");
            yield return new object[]{1};
            yield return new object[]{2};
        }

        [Theory]
        [MemberData(nameof(Provider), DisableDiscoveryEnumeration = true)]
        public void MemberDataNoEnumeration(int no)
        {
            Debug.WriteLine("MemberDataNoEnumeration");
        }
    }
}
