using FBUdpGeneratorAPP.Services;
using System;
using System.Windows.Controls;

namespace FBUdpGeneratorAPP.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReciverPage.xaml
    /// </summary>
    public partial class ReciverPage : Page
    {
        public ReciverPage()
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
                    value *= 2;
                }
                else
                {
                    value /= 2;
                }
               
                box.Text = value.ToString();
            }
        }
    }
}
