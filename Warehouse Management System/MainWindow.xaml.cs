﻿using System;
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
using Warehouse_Management_System.ViewModels;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;

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
            var products = db.Products.OrderBy(p => p.Id).ToList();
            ProductsGrid.ItemsSource = products;
            CheckLowQuantity(products);
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                var oldName = selectedProduct.Name;

                var editWindow = new Window();
                editWindow.Content = new EditProductWindow(selectedProduct);
                editWindow.Width = 300;
                editWindow.Height = 200;
                editWindow.Title = "Редактировать товар";
                editWindow.ShowDialog();

                using var db = new WarehouseDbContext();
                var products = db.Products.OrderBy(p => p.Id).ToList();
                ProductsGrid.ItemsSource = products;
                CheckLowQuantity(products);

                db.Logs.Add(new Log
                {
                    Operation = "Редактирование",
                    ProductName = oldName,
                    Timestamp = DateTime.UtcNow
                });
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования");
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsGrid.SelectedItem as Product;
            if (selectedProduct != null)
            {
                var productName = selectedProduct.Name;

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

                    db.Logs.Add(new Log
                    {
                        Operation = "Удаление",
                        ProductName = productName,
                        Timestamp = DateTime.UtcNow
                    });
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления");
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            using var db = new WarehouseDbContext();
            var products = db.Products
                .Where(p => p.Name.ToLower().Contains(searchText))
                .OrderBy(p => p.Id)
                .ToList();
            ProductsGrid.ItemsSource = products;
            CheckLowQuantity(products);
        }

        public void CheckLowQuantity(List<Product> products)
        {
            foreach (var product in products)
            {
                if (product.Quantity < 150)
                {
                    MessageBox.Show($"Критический остаток! Товар: {product.Name}, Количество: {product.Quantity} ");
                }
            }
        }

        private void LogHistory_Click(object sender, RoutedEventArgs e)
        {
            var logWindow = new Window();
            logWindow.Content = new LogWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = "История изменений";
            logWindow.ShowDialog();
        }
    }
}           