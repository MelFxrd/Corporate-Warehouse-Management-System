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
using Warehouse_Management_System.Models;
using Warehouse_Management_System.Data;

namespace Warehouse_Management_System
{
    public partial class AddProductWindow : UserControl
    {
        public Product NewProduct { get; set; }

        public AddProductWindow()
        {
            InitializeComponent();
            NewProduct = new Product();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка");
                NameTextBox.Focus();
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Количество должно быть целым числом не меньше 0", "Ошибка");
                QuantityTextBox.Focus();
                return;
            }

            if (!float.TryParse(PriceTextBox.Text.Replace(",", "."), out float price) || price < 0)
            {
                MessageBox.Show("Цена должна быть числом не меньше 0", "Ошибка");
                PriceTextBox.Focus();
                return;
            }

            NewProduct.Name = NameTextBox.Text.Trim();
            NewProduct.Quantity = quantity;
            NewProduct.Price = price;

            NewProduct.Category = "Без категории";
            if (CategoryComboBox.SelectedItem is ComboBoxItem selected && selected.Content != null)
            {
                NewProduct.Category = selected.Content.ToString();
            }

            using var db = new WarehouseDbContext();
            db.Products.Add(NewProduct);
            db.SaveChanges();

            db.Logs.Add(new Log
            {
                Operation = "Создание",
                ProductName = NewProduct.Name,
                Timestamp = DateTime.UtcNow
            });
            db.SaveChanges();

            Window.GetWindow(this)?.Close();
        }
    }
}