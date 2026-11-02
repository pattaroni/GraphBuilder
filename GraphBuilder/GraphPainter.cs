using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace GraphBuilder
{
    // Малює сітку, осі та графіки на Canvas
    public class GraphPainter
    {
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

            // Кількість поділок залежить від розміру полотна
            double xStep = GetTickStep(XMax - XMin, canvas.Bounds.Width / 70);
            double yStep = GetTickStep(YMax - YMin, canvas.Bounds.Height / 50);
            List<double> xTicks = GetTicks(XMin, XMax, xStep);
            List<double> yTicks = GetTicks(YMin, YMax, yStep);

            DrawGrid(xTicks, yTicks);
            DrawAxes(xTicks, yTicks);

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

        // Вибирає крок поділок виду 1, 2 або 5, помножене на степінь десятки
        private double GetTickStep(double range, double tickCount)
        {
            double roughStep = range / tickCount;
            double power = Math.Pow(10, Math.Floor(Math.Log10(roughStep)));
            double value = roughStep / power;

            if (value < 1.5)
            {
                return power;
            }
            if (value < 3.5)
            {
                return 2 * power;
            }
            if (value < 7.5)
            {
                return 5 * power;
            }
            return 10 * power;
        }

        // Значення поділок від min до max. Кожне рахуємо як номер * крок,
        // щоб не накопичувалась похибка
        private List<double> GetTicks(double min, double max, double step)
        {
            List<double> ticks = new List<double>();
            double first = Math.Ceiling(min / step);
            double last = Math.Floor(max / step);
            int count = (int)(last - first);

            for (int i = 0; i <= count; i++)
            {
                ticks.Add((first + i) * step);
            }

            return ticks;
        }

        private void DrawGrid(List<double> xTicks, List<double> yTicks)
        {
            foreach (double x in xTicks)
            {
                double screenX = ToScreenX(x);
                AddLine(screenX, 0, screenX, canvas.Bounds.Height, Brushes.LightGray, 1);
            }

            foreach (double y in yTicks)
            {
                double screenY = ToScreenY(y);
                AddLine(0, screenY, canvas.Bounds.Width, screenY, Brushes.LightGray, 1);
            }
        }

        private void DrawAxes(List<double> xTicks, List<double> yTicks)
        {
            double width = canvas.Bounds.Width;
            double height = canvas.Bounds.Height;

            // Якщо нуль не потрапляє в діапазон, вісь малюємо по краю полотна
            bool zeroInX = XMin <= 0 && XMax >= 0;
            bool zeroInY = YMin <= 0 && YMax >= 0;
            double axisX = 0;
            double axisY = height - 1;
            if (zeroInX)
            {
                axisX = ToScreenX(0);
            }
            if (zeroInY)
            {
                axisY = ToScreenY(0);
            }

            AddLine(0, axisY, width, axisY, Brushes.Black, 1.5);
            AddLine(axisX, 0, axisX, height, Brushes.Black, 1.5);

            DrawXTicks(xTicks, axisY, zeroInX);
            DrawYTicks(yTicks, axisX, zeroInY);

            // Підпис осі y зліва від осі, бо справа стоять числа
            double nameX = axisX - 12;
            if (nameX < 2)
            {
                nameX = axisX + 40;
            }
            AddLabel("x", width - 12, axisY - 20);
            AddLabel("y", nameX, 0);
        }

        private void DrawXTicks(List<double> xTicks, double axisY, bool zeroInX)
        {
            double width = canvas.Bounds.Width;

            // Підписи під віссю, а якщо внизу немає місця, то над нею
            double labelY = axisY + 5;
            if (labelY > canvas.Bounds.Height - 20)
            {
                labelY = axisY - 20;
            }

            foreach (double x in xTicks)
            {
                double screenX = ToScreenX(x);
                string text = FormatTick(x);
                AddLine(screenX, axisY - 4, screenX, axisY + 4, Brushes.Black, 1);

                double labelX = screenX - text.Length * 3;
                if (x == 0 && zeroInX)
                {
                    labelX = screenX + 4;
                }
                labelX = Math.Clamp(labelX, 2, Math.Max(2, width - text.Length * 7));
                AddLabel(text, labelX, labelY);
            }
        }

        private void DrawYTicks(List<double> yTicks, double axisX, bool zeroInY)
        {
            double width = canvas.Bounds.Width;
            double height = canvas.Bounds.Height;

            foreach (double y in yTicks)
            {
                double screenY = ToScreenY(y);
                AddLine(axisX - 4, screenY, axisX + 4, screenY, Brushes.Black, 1);

                // Нуль уже підписаний на осі X
                if (y == 0 && zeroInY)
                {
                    continue;
                }

                // Підписи справа від осі, а якщо там немає місця, то зліва
                string text = FormatTick(y);
                double labelX = axisX + 7;
                if (labelX > width - 50)
                {
                    labelX = axisX - 7 - text.Length * 7;
                }
                AddLabel(text, labelX, Math.Clamp(screenY - 8, 0, Math.Max(0, height - 16)));
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
            return value.ToString("G6", CultureInfo.InvariantCulture);
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
