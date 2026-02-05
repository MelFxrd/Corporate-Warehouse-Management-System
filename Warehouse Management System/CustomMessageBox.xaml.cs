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
                Owner = mainWindow,                                  
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Title = "Внимание",
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.SingleBorderWindow,
                SizeToContent = SizeToContent.Height,                 
                MinWidth = 380,
                MinHeight = 160,
                MaxWidth = 600
            };

            var box = new CustomMessageBox();

            if (box.MessageText != null)
            {
                box.MessageText.Text = message;
            }

            if (mainWindow != null)
            {
                win.Background = mainWindow.Background;
                win.Foreground = mainWindow.Foreground;

                if (box.MainBorder != null)
                {
                    box.MainBorder.Background = mainWindow.Background;
                    box.MainBorder.BorderBrush = new SolidColorBrush(Colors.Gray); 
                }

                if (box.MessageText != null)
                {
                    box.MessageText.Foreground = mainWindow.Foreground;
                }
            }

            win.Content = box;
            win.ShowDialog();
        }
    }
}