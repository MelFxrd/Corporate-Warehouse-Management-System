using System.Windows;

namespace Warehouse_Management_System.Views
{
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
