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
using System.Windows.Shapes;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;

namespace Warehouse_Management_System
{
    public partial class CategoryWindow : Window
    {
        public CategoryWindow()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            using var db = new WarehouseDbContext();
            CategoriesListBox.ItemsSource = db.Categories.ToList();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text)) return;

            using var db = new WarehouseDbContext();
            db.Categories.Add(new Category { Name = CategoryNameTextBox.Text });
            db.SaveChanges();
            CategoryNameTextBox.Clear();
            LoadCategories();
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is Category selected)
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Введите новое название для категории:",
                    "Редактирование",
                    selected.Name);

                if (!string.IsNullOrWhiteSpace(newName) && newName != selected.Name)
                {
                    using var db = new WarehouseDbContext();
                    var categoryToUpdate = db.Categories.Find(selected.Id);

                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.Name = newName;
                        db.SaveChanges();
                        LoadCategories();
                    }
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию из списка!");
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is Category selected)
            {
                using var db = new WarehouseDbContext();
                db.Categories.Remove(selected);
                db.SaveChanges();
                LoadCategories();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}