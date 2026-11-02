using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GraphBuilder
{
    // Вікно повідомлення з кнопкою OK або кнопками Так/Ні
    public partial class MessageWindow : Window
    {
        // true, якщо користувач натиснув «Так»
        public bool Result { get; set; }

        public MessageWindow()
        {
            InitializeComponent();
        }

        public MessageWindow(string title, string text, bool yesNo)
        {
            InitializeComponent();
            Title = title;
            MessageText.Text = text;

            OkButton.IsVisible = !yesNo;
            OkButton.IsDefault = !yesNo;
            OkButton.IsCancel = !yesNo;

            YesButton.IsVisible = yesNo;
            YesButton.IsDefault = yesNo;
            NoButton.IsVisible = yesNo;
            NoButton.IsCancel = yesNo;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
