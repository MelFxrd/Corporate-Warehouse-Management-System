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
            NewProduct.Name = NameTextBox.Text;
            NewProduct.Quantity = int.Parse(QuantityTextBox.Text);
            NewProduct.Price = float.Parse(PriceTextBox.Text);
            NewProduct.Category = "Без категории";
            if (CategoryComboBox.SelectedItem != null)
            {
                var selected = CategoryComboBox.SelectedItem as ComboBoxItem;
                if (selected != null)
                {
                    NewProduct.Category = selected.Content.ToString();
                }
            }

            using var db = new WarehouseDbContext();
            db.Products.Add(NewProduct);
            db.SaveChanges();

            var logEntry = new Log
            {
                Operation = "Создание",
                ProductName = NewProduct.Name,
                Timestamp = DateTime.UtcNow
            };
            db.Logs.Add(logEntry);
            db.SaveChanges();

            Window.GetWindow(this).Close();
        }
    }
}