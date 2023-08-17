using FBUdpGeneratorAPP.Services;
using System.Windows.Controls;

namespace FBUdpGeneratorAPP.Pages
{
    /// <summary>
    /// Логика взаимодействия для SenderPage.xaml
    /// </summary>
    public partial class SenderPage : Page
    {
        public SenderPage()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = InputValidator.IsNumeric(e.Text);
        }

        private void TextBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (sender is TextBox box && box.Text.Length > 0)
            {
                int value = int.Parse(box.Text);
                if (value == 0)
                {
                    value = 1024;
                }

                if (e.Delta > 0)
                {
                    value *= 10;
                }
                else
                {
                    value /= 10;
                }

                box.Text = value.ToString();
            }
        }
    }
}
