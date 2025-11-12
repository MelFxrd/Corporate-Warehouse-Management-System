using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Warehouse_Management_System.Data;

namespace Warehouse_Management_System
{
    public partial class ReportWindow : UserControl
    {
        public ISeries[] Series { get; set; }

        public ReportWindow()
        {
            InitializeComponent();
            LoadChart();
            DataContext = this;
        }

        private void LoadChart()
        {
            using var db = new WarehouseDbContext();

            var products = db.Products.ToList();
            var categories = new List<string>();
            var totals = new List<int>();

            foreach (var product in products)
            {
                var cat = product.Category ?? "Без категории";
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