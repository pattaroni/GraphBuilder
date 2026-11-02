using System.Collections.Generic;
using Avalonia.Controls;

namespace GraphBuilder
{
    public partial class MainWindow : Window
    {
        private List<Graph> graphs = new List<Graph>();
        private GraphPainter painter;

        public MainWindow()
        {
            InitializeComponent();
            painter = new GraphPainter(GraphCanvas);
        }

        // При зміні розміру вікна перемальовуємо полотно
        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            painter.Draw(graphs);
        }
    }
}
