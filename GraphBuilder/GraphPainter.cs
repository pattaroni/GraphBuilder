using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace GraphBuilder
{
    // Малює сітку, осі та графіки на Canvas
    public class GraphPainter
    {
        // На скільки частин ділимо кожну вісь
        private const int TickCount = 10;

        private Canvas canvas;

        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }

        public GraphPainter(Canvas canvas)
        {
            this.canvas = canvas;
            XMin = -10;
            XMax = 10;
            YMin = -10;
            YMax = 10;
        }

        public void Draw(List<Graph> graphs)
        {
            canvas.Children.Clear();

            if (canvas.Bounds.Width <= 0 || canvas.Bounds.Height <= 0)
            {
                return;
            }

            DrawGrid();
            DrawAxes();

            foreach (Graph graph in graphs)
            {
                DrawGraph(graph);
            }
        }

        // Переведення координат функції в пікселі полотна і назад
        public double ToScreenX(double x)
        {
            return (x - XMin) / (XMax - XMin) * canvas.Bounds.Width;
        }

        public double ToScreenY(double y)
        {
            return canvas.Bounds.Height - (y - YMin) / (YMax - YMin) * canvas.Bounds.Height;
        }

        public double ToWorldX(double screenX)
        {
            return XMin + screenX / canvas.Bounds.Width * (XMax - XMin);
        }

        public double ToWorldY(double screenY)
        {
            return YMax - screenY / canvas.Bounds.Height * (YMax - YMin);
        }

        private void DrawGrid()
        {
            double width = canvas.Bounds.Width;
            double height = canvas.Bounds.Height;

            for (int i = 0; i <= TickCount; i++)
            {
                double x = width * i / TickCount;
                double y = height * i / TickCount;
                AddLine(x, 0, x, height, Brushes.LightGray, 1);
                AddLine(0, y, width, y, Brushes.LightGray, 1);
            }
        }

        // Осі проходять через нуль. Якщо нуля немає в діапазоні, вісь малюється по краю полотна
        private void DrawAxes()
        {
            double width = canvas.Bounds.Width;
            double height = canvas.Bounds.Height;
            double axisX = Math.Clamp(ToScreenX(0), 0, width - 1);
            double axisY = Math.Clamp(ToScreenY(0), 0, height - 1);

            AddLine(0, axisY, width, axisY, Brushes.Black, 1.5);
            AddLine(axisX, 0, axisX, height, Brushes.Black, 1.5);

            for (int i = 0; i <= TickCount; i++)
            {
                double x = XMin + (XMax - XMin) * i / TickCount;
                double y = YMin + (YMax - YMin) * i / TickCount;
                double screenX = ToScreenX(x);
                double screenY = ToScreenY(y);

                // Поділки на осях
                AddLine(screenX, axisY - 4, screenX, axisY + 4, Brushes.Black, 1);
                AddLine(axisX - 4, screenY, axisX + 4, screenY, Brushes.Black, 1);

                // Числа ставимо так, щоб вони не виходили за край полотна
                AddLabel(FormatTick(x), Math.Min(screenX + 2, width - 45), Math.Min(axisY + 4, height - 18));

                // Нуль уже підписаний на осі X
                string yText = FormatTick(y);
                if (yText != "0")
                {
                    AddLabel(yText, Math.Min(axisX + 6, width - 45), Math.Min(screenY + 2, height - 18));
                }
            }
        }

        // Кожен суцільний шматок графіка малюємо окремою лінією
        private void DrawGraph(Graph graph)
        {
            SolidColorBrush brush = new SolidColorBrush(graph.Color);
            List<List<Point>> segments = graph.GetSegments(YMin, YMax);

            foreach (List<Point> segment in segments)
            {
                List<Point> screenPoints = new List<Point>();
                foreach (Point point in segment)
                {
                    screenPoints.Add(new Point(ToScreenX(point.X), ToScreenY(point.Y)));
                }

                Polyline line = new Polyline();
                line.Points = screenPoints;
                line.Stroke = brush;
                line.StrokeThickness = 2;
                canvas.Children.Add(line);
            }
        }

        private string FormatTick(double value)
        {
            double rounded = Math.Round(value, 3);
            // Щоб замість нуля не виводилось "-0"
            if (rounded == 0)
            {
                rounded = 0;
            }
            return rounded.ToString();
        }

        private void AddLine(double x1, double y1, double x2, double y2, IBrush brush, double thickness)
        {
            Line line = new Line();
            line.StartPoint = new Point(x1, y1);
            line.EndPoint = new Point(x2, y2);
            line.Stroke = brush;
            line.StrokeThickness = thickness;
            canvas.Children.Add(line);
        }

        private void AddLabel(string text, double x, double y)
        {
            TextBlock label = new TextBlock();
            label.Text = text;
            label.FontSize = 11;
            label.Foreground = Brushes.Black;
            Canvas.SetLeft(label, x);
            Canvas.SetTop(label, y);
            canvas.Children.Add(label);
        }
    }
}
