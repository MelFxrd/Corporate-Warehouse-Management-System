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
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Product.Name = NameTextBox.Text;
            Product.Quantity = int.Parse(QuantityTextBox.Text);
            Product.Price = float.Parse(PriceTextBox.Text);

            using var db = new WarehouseDbContext();
            db.Products.Update(Product);
            db.SaveChanges();

            Window.GetWindow(this).Close();
        }
    }
}