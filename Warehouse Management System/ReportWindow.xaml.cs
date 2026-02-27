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
using Microsoft.EntityFrameworkCore;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Warehouse_Management_System.Data;
using Warehouse_Management_System.Models;

namespace Warehouse_Management_System
{
    public partial class ReportWindow : UserControl
    {
        public ISeries[]? Series { get; set; }

        public ReportWindow()
        {
            InitializeComponent();
            LoadChart();
            DataContext = this;

            var fg = Application.Current.Resources["ForegroundBrush"] as SolidColorBrush;
            if (fg != null)
            {
                CategoryChart.LegendTextPaint = new SolidColorPaint(new SKColor(fg.Color.R, fg.Color.G, fg.Color.B));
            }
        }

        private void LoadChart()
        {
            using var db = new WarehouseDbContext();
            var products = db.Products.Include(p => p.Category).ToList();

            var categories = new List<string>();
            var totals = new List<int>();

            foreach (var product in products)
            {
                string cat = product.Category != null ? product.Category.Name : "Без категории";

                if (!categories.Contains(cat))
                {
                    categories.Add(cat);
                    totals.Add(0);
                }

                var index = categories.IndexOf(cat);
                totals[index] += product.Quantity;
            }

            var series = new List<PieSeries<int>>();
            var colors = new SKColor[]
            {
                new SKColor(255, 99, 132),
                new SKColor(54, 162, 235),
                new SKColor(255, 206, 86),
                new SKColor(75, 192, 192),
                new SKColor(153, 102, 255),
                new SKColor(255, 159, 64)
            };

            for (int i = 0; i < categories.Count; i++)
            {
                if (totals[i] > 0)
                {
                    series.Add(new PieSeries<int>
                    {
                        Name = categories[i],
                        Values = new[] { totals[i] },
                        Fill = new SolidColorPaint(colors[i % colors.Length])
                    });
                }
            }

            Series = series.ToArray();
        }
    }
}