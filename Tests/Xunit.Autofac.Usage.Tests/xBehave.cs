using System;
using Xbehave;
using Xunit.Abstractions;

namespace Xunit.Autofac.Usage.Tests
{
    public class XBehave
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ITestCase _testCase;

        public XBehave(ITestOutputHelper testOutputHelper, ITestCase testCase)
        {
            _testOutputHelper = testOutputHelper;
            _testCase = testCase;
        }

        [Scenario]
        public void Addition(int x, int y, Calculator calculator, int answer)
        {
            "Given the number 1"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    x = 1;
                });

            "And the number 2"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    y = 2;
                });

            "And a calculator"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    calculator = new Calculator();
                });

            "When I add the numbers together"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    answer = calculator.Add(x, y);
                });

            "Then the answer is 3"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    Assert.Equal(3, answer);
                });
        }


        [Scenario]
        [Example(1, 2, 3)]
        [Example(2, 3, 5)]
        public void AdditionByExample(int x, int y, int expectedAnswer, Calculator calculator, int answer)
        {
            $"Given the number {x}"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                });

            $"And the number {y}"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                });

            "And a calculator"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    calculator = new Calculator();
                });

            "When I add the numbers together"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    answer = calculator.Add(x, y);
                });

            $"Then the answer is {expectedAnswer}"
                .x(() =>
                {
                    _testOutputHelper.WriteLine($"{_testCase.GetType().Name}: {_testCase.DisplayName}");
                    Assert.Equal(expectedAnswer, answer);
                });
        }
    }

    public class Calculator
    {
        public int Add(int x, int y) => x + y;
    }
}
