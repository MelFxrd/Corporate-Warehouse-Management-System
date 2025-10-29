using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse_Management_System.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string ProductName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}