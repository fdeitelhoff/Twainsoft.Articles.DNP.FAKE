using FluentAssertions;
using NUnit.Framework;
using Twainsoft.SimpleApplication.Lib;

namespace Twainsoft.SimpleApplication.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void AddTest()
        {
            // Arrange
            const int a = 5;
            const int b = 5;
            const int expected = a + b;

            // Act
            var result = Calculator.Add(a, b);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void MultTest()
        {
            // Arrange
            const int a = 5;
            const int b = 5;
            const int expected = a*b;

            // Act
            var result = Calculator.Mult(a, b);

            // Assert
            result.Should().Be(expected);
        }
    }
}
