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

            using var db = new WarehouseDbContext();
            var products = db.Products.ToList();
            ProductsGrid.ItemsSource = products;
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                using var db = new WarehouseDbContext();

                var productDb = db.Products.Find(selectedProduct.Id);
                if (productDb != null)
                {
                    db.Products.Remove(productDb);
                    db.SaveChanges();

                    var currentList = ProductsGrid.ItemsSource as List<Product>;
                    if (currentList != null)
                    {
                        currentList.Remove(selectedProduct);
                        ProductsGrid.ItemsSource = null;
                        ProductsGrid.ItemsSource = currentList;
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления");
            }
        }
        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                var editWindow = new Window();
                editWindow.Content = new EditProductWindow(selectedProduct);
                editWindow.Width = 300;
                editWindow.Height = 200;
                editWindow.Title = "Редактировать товар";
                editWindow.ShowDialog();

                using var db = new WarehouseDbContext();
                var products = db.Products.OrderBy(p => p.Id).ToList();
                ProductsGrid.ItemsSource = products;
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования");
            }
        }
    } 
}