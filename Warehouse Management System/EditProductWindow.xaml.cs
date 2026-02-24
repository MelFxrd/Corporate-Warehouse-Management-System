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

            using var db = new WarehouseDbContext();
            var categories = db.Categories.ToList();
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "Name";

            foreach (var item in categories)
            {
                if (item.Id == product.CategoryId)
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
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Ошибка в количестве");
                return;
            }

            if (!float.TryParse(PriceTextBox.Text.Replace(",", "."), out float price) || price < 0)
            {
                MessageBox.Show("Ошибка в цене");
                return;
            }

            if (CategoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            using (var db = new WarehouseDbContext())
            {
                var productInDb = db.Products.Find(Product.Id);
                if (productInDb != null)
                {
                    productInDb.Name = NameTextBox.Text.Trim();
                    productInDb.Quantity = quantity;
                    productInDb.Price = price;
                    productInDb.CategoryId = ((Category)CategoryComboBox.SelectedItem).Id;

                    db.SaveChanges();
                }
            }

            Window.GetWindow(this)?.Close();
        }
    }
}