using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse_Management_System.ViewModels
{
    public class ProductViewModel
    {
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        public ProductViewModel()
        {
            RefreshProducts();
        }

        public void RefreshProducts()
        {
            Products.Clear();
            using var db = new WarehouseDbContext();
            var products = db.Products.ToList();
            foreach (var p in products) Products.Add(p);
        }
    }
}
