using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace GraphBuilder
{
    // Вікно з таблицею значень вибраного графіка
    public partial class ValuesWindow : Window
    {
        public ValuesWindow()
        {
            InitializeComponent();
        }

        public ValuesWindow(Graph graph)
        {
            InitializeComponent();
            FormulaText.Text = graph.Function.Name + ": " + graph.Function.GetFormula();
            HeaderText.Text = "x".PadRight(20) + "y";

            List<string> rows = new List<string>();
            foreach (Point point in graph.Points)
            {
                rows.Add(InputHelper.FormatNumber(point.X).PadRight(20) + InputHelper.FormatNumber(point.Y));
            }

            ValuesListBox.ItemsSource = rows;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
