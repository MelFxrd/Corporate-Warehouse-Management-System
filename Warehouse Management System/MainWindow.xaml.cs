using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;
using Warehouse_Management_System.ViewModels;
using Warehouse_Management_System.Views;

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
            var addWindow = new Window
            {
                Background = Background,
                Foreground = Foreground,
                Content = new AddProductWindow(),
                Width = 300,
                Height = 250,
                Title = "Добавить товар"
            };
            addWindow.ShowDialog();

            using var db = new WarehouseDbContext();
            var products = db.Products.OrderBy(p => p.Id).ToList();
            ProductsGrid.ItemsSource = products;
            CheckLowQuantity(products);
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is not Product selectedProduct)
            {
                CustomMessageBox.Show("Выберите товар для редактирования");
                return;
            }

            var oldName = selectedProduct.Name;

            var editWindow = new Window
            {
                Background = Background,
                Foreground = Foreground,
                Content = new EditProductWindow(selectedProduct),
                Width = 300,
                Height = 250,
                Title = "Редактировать товар"
            };
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

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is not Product selectedProduct)
            {
                CustomMessageBox.Show("Выберите товар для удаления");
                return;
            }

            var confirm = new ConfirmDialog(
                $"Вы действительно хотите удалить товар:\n\n«{selectedProduct.Name}»?"
            )
            {
                Owner = this
            };

            bool? result = confirm.ShowDialog();

            if (result != true)
                return;

            var productName = selectedProduct.Name;

            using var db = new WarehouseDbContext();
            var productDb = db.Products.Find(selectedProduct.Id);
            if (productDb == null) return;

            db.Products.Remove(productDb);
            db.SaveChanges();

            if (ProductsGrid.ItemsSource is List<Product> currentList)
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.Trim().ToLower();

            string selectedCategory = "Все";
            if (CategoryFilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedCategory = selectedItem.Content.ToString();
            }

            using var db = new WarehouseDbContext();
            var query = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.Name.ToLower().Contains(searchText));
            }

            if (selectedCategory != "Все")
            {
                query = query.Where(p => p.Category == selectedCategory);
            }

            var products = query.OrderBy(p => p.Id).ToList();
            ProductsGrid.ItemsSource = products;
            CheckLowQuantity(products);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                CategoryFilterComboBox.SelectedIndex = 0;
                ResetSearch_Click(null, null);
            }
        }


        private void ResetSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            CategoryFilterComboBox.SelectedIndex = 0;

            using var db = new WarehouseDbContext();
            var products = db.Products
                             .OrderBy(p => p.Id)
                             .ToList();

            ProductsGrid.ItemsSource = products;
            CheckLowQuantity(products);
        }


        public void CheckLowQuantity(List<Product> products)
        {
            foreach (var product in products.Where(p => p.Quantity < 150))
            {
                CustomMessageBox.Show($"Критический остаток!\nТовар: {product.Name}\nКоличество: {product.Quantity}");
            }
        }

        private void LogHistory_Click(object sender, RoutedEventArgs e)
        {
            new Window
            {
                Background = Background,
                Foreground = Foreground,
                Content = new LogWindow(),
                Width = 700,
                Height = 500,
                Title = "История изменений"
            }.ShowDialog();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            new Window
            {
                Background = Background,
                Foreground = Foreground,
                Content = new ReportWindow(),
                Width = 900,
                Height = 600,
                Title = "Отчёт по категориям"
            }.ShowDialog();
        }

        private async void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                List<Product> products;

                using (var db = new WarehouseDbContext())
                {
                    products = await db.Products.OrderBy(p => p.Id).ToListAsync(); 
                }

                if (products == null || products.Count == 0)
                {
                    CustomMessageBox.Show("Нет товаров для экспорта");
                    return;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = System.IO.Path.Combine(desktopPath, "Отчёт_склада.xlsx");

                await Task.Run(() =>
                {
                    using var wb = new XLWorkbook();
                    var ws = wb.Worksheets.Add("Товары");

                    ws.Cell(1, 1).Value = "ID";
                    ws.Cell(1, 2).Value = "Название";
                    ws.Cell(1, 3).Value = "Количество";
                    ws.Cell(1, 4).Value = "Цена";
                    ws.Cell(1, 5).Value = "Категория";

                    for (int i = 0; i < products.Count; i++)
                    {
                        var p = products[i];
                        ws.Cell(i + 2, 1).Value = p.Id;
                        ws.Cell(i + 2, 2).Value = p.Name;
                        ws.Cell(i + 2, 3).Value = p.Quantity;
                        ws.Cell(i + 2, 4).Value = p.Price;
                        ws.Cell(i + 2, 5).Value = p.Category ?? "Без категории";
                    }

                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Fill.BackgroundColor = XLColor.White;
                    ws.Columns(1, 5).AdjustToContents();

                    wb.SaveAs(filePath);
                });

                CustomMessageBox.Show($"Готово!\nФайл сохранён:\n{filePath}");
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"Ошибка при экспорте:\n{ex.Message}");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative) });
            ThemeToggle.Content = "Светлая тема";
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary { Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative) });
            ThemeToggle.Content = "Тёмная тема";
        }
    }
}