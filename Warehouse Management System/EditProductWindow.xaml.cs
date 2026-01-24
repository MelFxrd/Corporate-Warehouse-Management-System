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
    public partial class EditProductWindow : UserControl
    {
        public Product Product { get; set; }

        public EditProductWindow(Product product)
        {
            InitializeComponent();
            Product = product;
            NameTextBox.Text = product.Name;
            QuantityTextBox.Text = product.Quantity.ToString();
            PriceTextBox.Text = product.Price.ToString();

            foreach (ComboBoxItem item in CategoryComboBox.Items)
            {
                if (item.Content.ToString() == product.Category)
                {
                    CategoryComboBox.SelectedItem = item;
                    break;
                }
            }
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

            Product.Name = NameTextBox.Text.Trim();
            Product.Quantity = quantity;
            Product.Price = price;

            Product.Category = "Без категории";
            if (CategoryComboBox.SelectedItem is ComboBoxItem selected && selected.Content != null)
            {
                Product.Category = selected.Content.ToString();
            }

            using var db = new WarehouseDbContext();
            db.Products.Update(Product);
            db.SaveChanges();

            Window.GetWindow(this)?.Close();
        }
    }
}