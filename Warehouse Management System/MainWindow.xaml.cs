using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Window();
            addWindow.Content = new AddProductWindow();
            addWindow.Width = 300;
            addWindow.Height = 200;
            addWindow.Title = "Добавить товар";
            addWindow.ShowDialog();
            var control = addWindow.Content as AddProductWindow;
            if (control != null)
            {
                using var db = new WarehouseDbContext();
                db.Products.Add(control.NewProduct);
                db.SaveChanges();
                var viewModel = DataContext as ProductViewModel;
                viewModel.LoadProducts();
                ProductsGrid.ItemsSource = viewModel.Products;
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                using var db = new WarehouseDbContext();
                db.Products.Remove(selectedProduct);
                db.SaveChanges();
                var viewModel = DataContext as ProductViewModel;
                viewModel.LoadProducts();
                ProductsGrid.ItemsSource = viewModel.Products;
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления");
            }
        }
    }
}