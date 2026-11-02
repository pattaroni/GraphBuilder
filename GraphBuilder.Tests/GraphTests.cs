using System;
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

            // Великий діапазон Y: перевіряємо, що рветься саме на асимптотах,
            // а не через вихід за межі
            List<List<Point>> segments = graph.GetSegments(-1000, 1000);

            Assert.Equal(3, segments.Count);
            foreach (List<Point> segment in segments)
            {
                double first = segment[0].X;
                double last = segment[segment.Count - 1].X;
                Assert.False(first < Math.PI / 2 && last > Math.PI / 2);
                Assert.False(first < -Math.PI / 2 && last > -Math.PI / 2);
            }
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

        [Fact]
        public void FarPoints_AreNotDrawn()
        {
            Graph graph = new Graph(new QuadraticFunction(1, 0, 0), Colors.Blue);
            graph.CalculatePoints(-100, 100, 1);

            // Межа 201 за модулем, тому лишаються точки від x = -14 до x = 14
            List<List<Point>> segments = graph.GetSegments(-1, 1);

            Assert.Single(segments);
            Assert.Equal(29, segments[0].Count);
        }
    }
}
