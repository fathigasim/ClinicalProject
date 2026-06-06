using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectTest
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_TwoNumbers_ReturnsCorrectSum()
        {
            // Arrange — set up inputs
            var a = 5;
            var b = 3;

            // Act — call the thing you're testing
            var result = a + b;

            // Assert — verify the outcome
            result.Should().Be(8);
        }
    }
}
