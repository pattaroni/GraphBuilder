using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

            // Підписуємось тут, а не в axaml, бо під час завантаження розмітки
            // поля ще не створені, а обробник до них звертається
            FunctionComboBox.SelectionChanged += FunctionComboBox_SelectionChanged;
            ATextBox.TextChanged += Coefficient_TextChanged;
            BTextBox.TextChanged += Coefficient_TextChanged;
            CTextBox.TextChanged += Coefficient_TextChanged;

            UpdateFormula();
        }

        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            BuildGraph();
        }

        private void BuildGraph()
        {
            // Порядок полів: a, b, c, Xmin, Xmax, крок, Ymin, Ymax
            double[] values = new double[8];
            if (!ReadFields(values))
            {
                return;
            }

            double xMin = values[3];
            double xMax = values[4];
            double step = values[5];
            double yMin = values[6];
            double yMax = values[7];

            string error = InputHelper.CheckRange(xMin, xMax, step, yMin, yMax);
            if (error != null)
            {
                ShowError(error);
                return;
            }

            Graph graph = new Graph(CreateFunction(values[0], values[1], values[2]), colors[colorIndex]);
            graph.CalculatePoints(xMin, xMax, step);

            if (!graph.HasValidPoints())
            {
                ShowError("Функція не визначена на вибраному діапазоні X.");
                return;
            }

            colorIndex = (colorIndex + 1) % colors.Length;
            graphs.Add(graph);

            // Рядок у списку того самого кольору, що й графік
            TextBlock item = new TextBlock();
            item.Text = graph.Function.Name + ": " + graph.Function.GetFormula();
            item.Foreground = new SolidColorBrush(graph.Color);
            item.TextWrapping = TextWrapping.Wrap;
            GraphsListBox.Items.Add(item);
            GraphsListBox.SelectedIndex = GraphsListBox.Items.Count - 1;

            // Діапазон міг змінитись, тому перераховуємо всі графіки
            foreach (Graph g in graphs)
            {
                g.CalculatePoints(xMin, xMax, step);
            }

            painter.XMin = xMin;
            painter.XMax = xMax;
            painter.YMin = yMin;
            painter.YMax = yMax;
            painter.Draw(graphs);
        }

        // Читає числа з полів. Якщо поле заповнене неправильно, показує помилку і повертає false
        private bool ReadFields(double[] values)
        {
            TextBox[] boxes = { ATextBox, BTextBox, CTextBox, XMinTextBox, XMaxTextBox, StepTextBox, YMinTextBox, YMaxTextBox };
            string[] names = { "a", "b", "c", "Xmin", "Xmax", "Крок", "Ymin", "Ymax" };

            for (int i = 0; i < boxes.Length; i++)
            {
                // Неактивне поле функції не потрібне
                if (!boxes[i].IsEnabled)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(boxes[i].Text))
                {
                    ShowError("Поле «" + names[i] + "» порожнє. Введіть число.");
                    boxes[i].Focus();
                    return false;
                }

                if (!InputHelper.TryParseNumber(boxes[i].Text, out values[i]))
                {
                    ShowError("У полі «" + names[i] + "» має бути число.");
                    boxes[i].Focus();
                    return false;
                }
            }

            return true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedGraph();
        }

        private void DeleteSelectedGraph()
        {
            int index = GraphsListBox.SelectedIndex;
            if (index < 0)
            {
                ShowError("Виберіть графік у списку.");
                return;
            }

            graphs.RemoveAt(index);
            GraphsListBox.Items.RemoveAt(index);
            GraphsListBox.SelectedIndex = Math.Min(index, graphs.Count - 1);
            painter.Draw(graphs);
        }

        // Тут потрібен await, щоб дочекатися відповіді Так/Ні
        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (graphs.Count == 0)
            {
                ShowError("Список графіків порожній.");
                return;
            }

            MessageWindow window = new MessageWindow("Підтвердження", "Видалити всі графіки?", true);
            await window.ShowDialog(this);

            if (window.Result)
            {
                graphs.Clear();
                GraphsListBox.Items.Clear();
                colorIndex = 0;
                painter.Draw(graphs);
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            string text = "Виберіть функцію, введіть коефіцієнти і діапазон, натисніть «Побудувати». "
                + "Можна будувати кілька графіків, кожен своїм кольором. "
                + "Числа можна писати і з комою, і з крапкою.\n\n"
                + "Enter — побудувати графік\n"
                + "Delete — видалити вибраний графік\n"
                + "F1 — довідка\n"
                + "Esc — закрити вікно\n\n"
                + "Курсовий проєкт «Побудова графіків функцій».\n"
                + "Виконав: Кирик О., гр. 585-КБ.";

            MessageWindow window = new MessageWindow("Довідка", text, false);
            window.ShowDialog(this);
        }

        private void ShowError(string text)
        {
            MessageWindow window = new MessageWindow("Помилка", text, false);
            window.ShowDialog(this);
        }

        // Гарячі клавіші головного вікна
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                BuildGraph();
            }
            else if (e.Key == Key.F1)
            {
                e.Handled = true;
                ShowHelp();
            }
            else if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                // На Mac клавіша delete працює як Back.
                // У полі введення ці клавіші мають стирати символи
                if (!(e.Source is TextBox))
                {
                    e.Handled = true;
                    DeleteSelectedGraph();
                }
            }
        }

        // Показує формулу з підставленими коефіцієнтами
        private void UpdateFormula()
        {
            // Лінійній функції коефіцієнт c не потрібен
            CTextBox.IsEnabled = FunctionComboBox.SelectedIndex != 0;

            double a = 0;
            double b = 0;
            double c = 0;
            bool correct = InputHelper.TryParseNumber(ATextBox.Text, out a)
                && InputHelper.TryParseNumber(BTextBox.Text, out b)
                && (!CTextBox.IsEnabled || InputHelper.TryParseNumber(CTextBox.Text, out c));

            if (correct)
            {
                FormulaText.Text = CreateFunction(a, b, c).GetFormula();
            }
            else
            {
                FormulaText.Text = "Некоректні коефіцієнти";
            }
        }

        private void FunctionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFormula();
        }

        private void Coefficient_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFormula();
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

        // Координати курсора в рядку стану
        private void GraphCanvas_PointerMoved(object sender, PointerEventArgs e)
        {
            Point position = e.GetPosition(GraphCanvas);
            double x = Math.Round(painter.ToWorldX(position.X), 2);
            double y = Math.Round(painter.ToWorldY(position.Y), 2);
            StatusText.Text = "x = " + x + "    y = " + y;
        }

        private void GraphCanvas_PointerExited(object sender, PointerEventArgs e)
        {
            StatusText.Text = "Готово";
        }

        // При зміні розміру вікна перемальовуємо полотно
        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            painter.Draw(graphs);
        }
    }
}
