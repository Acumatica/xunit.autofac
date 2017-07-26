using System;

namespace Xunit.Autofac.Usage.Tests
{
    public class SimpleTests
    {
        [Fact]
        public void Fact()
        {
            
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Theory(int no)
        {
            
        }
    }
}
