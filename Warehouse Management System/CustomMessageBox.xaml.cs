using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Warehouse_Management_System
{
    public partial class CustomMessageBox : UserControl
    {
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        public static void Show(string message)
        {
            var mainWindow = Application.Current.MainWindow;

            var win = new Window
            {
                Width = 420,
                Height = 220,
                Title = "Внимание",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = mainWindow.Background,
                Foreground = mainWindow.Foreground,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.SingleBorderWindow
            };

            var box = new CustomMessageBox();
            box.MessageText.Text = message;
            box.MainBorder.Background = mainWindow.Background;
            box.MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(136, 136, 136));
            box.MessageText.Foreground = mainWindow.Foreground;

            win.Content = box;
            win.ShowDialog();
        }
    }
}