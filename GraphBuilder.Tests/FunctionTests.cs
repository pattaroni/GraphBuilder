using System;
using GraphBuilder.Functions;
using Xunit;

namespace GraphBuilder.Tests
{
    public class FunctionTests
    {
        [Fact]
        public void Linear_CalculatesValue()
        {
            LinearFunction function = new LinearFunction(2, 3);
            Assert.Equal(11, function.Calculate(4), 6);
        }

        [Fact]
        public void Quadratic_CalculatesValue()
        {
            QuadraticFunction function = new QuadraticFunction(1, 2, 1);
            Assert.Equal(16, function.Calculate(3), 6);
        }

        [Fact]
        public void Sine_CalculatesValue()
        {
            SineFunction function = new SineFunction(2, 1, 1);
            Assert.Equal(3, function.Calculate(Math.PI / 2), 6);
        }

        [Fact]
        public void Hyperbola_AtAsymptote_ReturnsInfinity()
        {
            HyperbolaFunction function = new HyperbolaFunction(1, 2, 0);
            Assert.True(double.IsInfinity(function.Calculate(2)));
        }

        [Fact]
        public void Root_OfNegativeNumber_ReturnsNaN()
        {
            RootFunction function = new RootFunction(1, 1, 0);
            Assert.True(double.IsNaN(function.Calculate(-4)));
            Assert.Equal(2, function.Calculate(4), 6);
        }

        [Fact]
        public void Quadratic_Formula_ShowsCoefficients()
        {
            QuadraticFunction function = new QuadraticFunction(2, -3, 1);
            Assert.Equal("y = 2·x² − 3·x + 1", function.GetFormula());
        }
    }
}
