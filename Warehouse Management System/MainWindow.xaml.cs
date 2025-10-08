using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;
using Warehouse_Management_System.ViewModels;

namespace Warehouse_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ProductViewModel();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            using var db = new WarehouseDbContext();
            db.Products.Add(new Product { Name = "Тестовый продукт", Quantity = 10, Price = 100 });
            db.SaveChanges();
            if (DataContext is ProductViewModel viewModel)
            {
                viewModel.RefreshProducts();
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Изменить товар");
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удалить товар");
        }
    }
}