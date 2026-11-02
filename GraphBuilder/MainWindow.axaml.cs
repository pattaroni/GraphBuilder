using System.Collections.Generic;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using GraphBuilder.Functions;

namespace GraphBuilder
{
    public partial class MainWindow : Window
    {
        private List<Graph> graphs = new List<Graph>();
        private GraphPainter painter;

        // Кольори графіків беруться по черзі
        private Color[] colors = { Colors.Blue, Colors.Red, Colors.Green, Colors.DarkOrange, Colors.Purple, Colors.Teal };
        private int colorIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            painter = new GraphPainter(GraphCanvas);
        }

        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            double a = double.Parse(ATextBox.Text, CultureInfo.InvariantCulture);
            double b = double.Parse(BTextBox.Text, CultureInfo.InvariantCulture);
            double c = double.Parse(CTextBox.Text, CultureInfo.InvariantCulture);
            double xMin = double.Parse(XMinTextBox.Text, CultureInfo.InvariantCulture);
            double xMax = double.Parse(XMaxTextBox.Text, CultureInfo.InvariantCulture);
            double step = double.Parse(StepTextBox.Text, CultureInfo.InvariantCulture);
            double yMin = double.Parse(YMinTextBox.Text, CultureInfo.InvariantCulture);
            double yMax = double.Parse(YMaxTextBox.Text, CultureInfo.InvariantCulture);

            Graph graph = new Graph(CreateFunction(a, b, c), colors[colorIndex]);
            colorIndex = (colorIndex + 1) % colors.Length;
            graphs.Add(graph);

            painter.XMin = xMin;
            painter.XMax = xMax;
            painter.YMin = yMin;
            painter.YMax = yMax;

            // Діапазон міг змінитись, тому перераховуємо всі графіки
            foreach (Graph item in graphs)
            {
                item.CalculatePoints(xMin, xMax, step);
            }

            painter.Draw(graphs);
        }

        // Створює функцію за вибраним пунктом списку
        private MathFunction CreateFunction(double a, double b, double c)
        {
            switch (FunctionComboBox.SelectedIndex)
            {
                case 0:
                    return new LinearFunction(a, b);
                case 1:
                    return new QuadraticFunction(a, b, c);
                case 2:
                    return new CubicFunction(a, b, c);
                case 3:
                    return new SineFunction(a, b, c);
                case 4:
                    return new CosineFunction(a, b, c);
                case 5:
                    return new TangentFunction(a, b, c);
                case 6:
                    return new HyperbolaFunction(a, b, c);
                case 7:
                    return new ExponentialFunction(a, b, c);
                case 8:
                    return new LogarithmicFunction(a, b, c);
                default:
                    return new RootFunction(a, b, c);
            }
        }

        // При зміні розміру вікна перемальовуємо полотно
        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            painter.Draw(graphs);
        }
    }
}
