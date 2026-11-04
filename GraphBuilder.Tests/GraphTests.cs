using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using GraphBuilder.Functions;
using Xunit;

namespace GraphBuilder.Tests
{
    public class GraphTests
    {
        [Fact]
        public void CalculatePoints_GivesRightCount()
        {
            Graph graph = new Graph(new LinearFunction(1, 0), Colors.Blue);
            graph.CalculatePoints(-10, 10, 0.1);

            Assert.Equal(201, graph.Points.Count);
            Assert.Equal(10, graph.Points[200].X, 6);
        }

        [Fact]
        public void Tangent_IsSplitOnAsymptotes()
        {
            Graph graph = new Graph(new TangentFunction(1, 1, 0), Colors.Blue);
            graph.CalculatePoints(-2, 2, 0.1);

            // Від -2 до 2 є дві асимптоти (±π/2), тому має бути три шматки лінії
            Assert.Equal(3, graph.GetSegments(-1000, 1000).Count);
        }

        [Fact]
        public void Hyperbola_IsSplitOnAsymptote()
        {
            // Асимптота x = 0.25 не потрапляє на сітку точок
            Graph graph = new Graph(new HyperbolaFunction(1, 0.25, 0), Colors.Blue);
            graph.CalculatePoints(-5, 5, 0.5);

            Assert.Equal(2, graph.GetSegments(-1000, 1000).Count);
        }

        [Fact]
        public void Logarithm_SkipsUndefinedPoints()
        {
            Graph graph = new Graph(new LogarithmicFunction(1, 1, 0), Colors.Blue);
            graph.CalculatePoints(-5, 5, 1);

            List<List<Point>> segments = graph.GetSegments(-10, 10);

            Assert.True(graph.HasValidPoints());
            Assert.Single(segments);
            Assert.Equal(5, segments[0].Count);
        }
    }
}
