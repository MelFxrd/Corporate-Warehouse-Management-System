using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;

namespace Warehouse_Management_System.ViewModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }

        public ProductViewModel()
        {
            Products = new List<Product>();
            LoadProducts();
        }

        public void LoadProducts()
        {
            using var db = new WarehouseDbContext();
            Products = db.Products.ToList();
        }
    }
}