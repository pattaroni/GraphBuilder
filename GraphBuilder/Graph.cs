using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using GraphBuilder.Functions;

namespace GraphBuilder
{
    // Графік: функція, колір і обчислені точки
    public class Graph
    {
        public MathFunction Function { get; set; }
        public Color Color { get; set; }
        public List<Point> Points { get; set; }

        public Graph(MathFunction function, Color color)
        {
            Function = function;
            Color = color;
            Points = new List<Point>();
        }

        public void CalculatePoints(double xMin, double xMax, double step)
        {
            Points.Clear();

            // Малий доданок потрібен, бо 20 / 0.1 може дати 199.99999999
            int count = (int)Math.Floor((xMax - xMin) / step + 1e-9);

            for (int i = 0; i <= count; i++)
            {
                // x рахуємо через номер точки, щоб не накопичувалась похибка
                double x = Math.Round(xMin + i * step, 12);
                double y = Function.Calculate(x);
                Points.Add(new Point(x, y));
            }
        }

        // Чи є хоча б одна точка, де функція визначена
        public bool HasValidPoints()
        {
            foreach (Point point in Points)
            {
                if (double.IsFinite(point.Y))
                {
                    return true;
                }
            }

            return false;
        }

        // Ділить точки на суцільні шматки лінії. Лінія переривається на NaN/Infinity,
        // на точках далеко за межами полотна і на асимптотах
        public List<List<Point>> GetSegments(double yMin, double yMax)
        {
            double height = yMax - yMin;
            double low = yMin - 100 * height;
            double high = yMax + 100 * height;

            List<List<Point>> segments = new List<List<Point>>();
            List<Point> current = new List<Point>();

            for (int i = 0; i < Points.Count; i++)
            {
                Point point = Points[i];

                if (!double.IsFinite(point.Y) || point.Y < low || point.Y > high)
                {
                    FinishSegment(segments, current);
                    current = new List<Point>();
                    continue;
                }

                if (current.Count > 0 && Function.HasBreakBetween(Points[i - 1].X, point.X))
                {
                    FinishSegment(segments, current);
                    current = new List<Point>();
                }

                current.Add(point);
            }

            FinishSegment(segments, current);
            return segments;
        }

        // Шматок з однієї точки лінією не намалювати, тому він не потрібен
        private void FinishSegment(List<List<Point>> segments, List<Point> segment)
        {
            if (segment.Count >= 2)
            {
                segments.Add(segment);
            }
        }
    }
}
