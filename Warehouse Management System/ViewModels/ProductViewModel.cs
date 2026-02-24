using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;

namespace Warehouse_Management_System.ViewModels
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public ProductViewModel()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
            LoadData();
        }

        public void LoadData()
        {
            using var db = new WarehouseDbContext();
            Categories = db.Categories.ToList();
            Products = db.Products.Include(p => p.Category).ToList();
        }
    }
}