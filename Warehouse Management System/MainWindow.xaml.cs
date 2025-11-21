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
using Warehouse_Management_System.ViewModels;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

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
            addWindow.Height = 250;
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
                editWindow.Height = 250;
                editWindow.Title = "Редактировать товар";
                editWindow.ShowDialog();

                using var db = new WarehouseDbContext();
                var products = db.Products.OrderBy(p => p.Id).ToList();
                ProductsGrid.ItemsSource = products;
                CheckLowQuantity(products);

                var logEntry = new Log
                {
                    Operation = "Редактирование",
                    ProductName = oldName,
                    Timestamp = DateTime.UtcNow
                };
                db.Logs.Add(logEntry);
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

                    var logEntry = new Log
                    {
                        Operation = "Удаление",
                        ProductName = productName,
                        Timestamp = DateTime.UtcNow
                    };
                    db.Logs.Add(logEntry);
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

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = "Все";

            if (CategoryFilterComboBox.SelectedItem != null)
            {
                var selectedItem = CategoryFilterComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    selectedCategory = selectedItem.Content.ToString();
                }
            }

            using var db = new WarehouseDbContext();
            var products = db.Products.ToList();

            if (selectedCategory != "Все")
            {
                products = products.Where(p => p.Category == selectedCategory).ToList();
            }

            ProductsGrid.ItemsSource = products.OrderBy(p => p.Id).ToList();
            CheckLowQuantity(products);
        }

        public void CheckLowQuantity(List<Product> products)
        {
            foreach (var product in products)
            {
                if (product.Quantity < 150)
                {
                    MessageBox.Show($"Критический остаток! Товар: {product.Name}, Количество: {product.Quantity}");
                }
            }
        }

        private void LogHistory_Click(object sender, RoutedEventArgs e)
        {
            var logWindow = new Window();
            logWindow.Content = new LogWindow();
            logWindow.Width = 700;
            logWindow.Height = 500;
            logWindow.Title = "История изменений";
            logWindow.ShowDialog();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window
            {
                Title = "Отчёт по категориям",
                Width = 900,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            win.Content = new ReportWindow();
            win.ShowDialog();
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            using var db = new WarehouseDbContext();
            var products = db.Products.OrderBy(p => p.Id).ToList();

            var file = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Отчёт_склада.xlsx");

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Товары");

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Название";
            ws.Cell(1, 3).Value = "Количество";
            ws.Cell(1, 4).Value = "Цена";
            ws.Cell(1, 5).Value = "Категория";

            for (int i = 0; i < products.Count; i++)
            {
                ws.Cell(i + 2, 1).Value = products[i].Id;
                ws.Cell(i + 2, 2).Value = products[i].Name;
                ws.Cell(i + 2, 3).Value = products[i].Quantity;
                ws.Cell(i + 2, 4).Value = products[i].Price;
                ws.Cell(i + 2, 5).Value = products[i].Category ?? "Без категории";
            }

            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Fill.BackgroundColor = XLColor.White;
            ws.Columns(1, 5).AdjustToContents();

            wb.SaveAs(file);
            MessageBox.Show("Готово! Файл на рабочем столе: Отчёт_склада.xlsx");
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative) });
            ThemeToggle.Content = "Светлая тема";
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative) });
            ThemeToggle.Content = "Тёмная тема";
        }
    }
}