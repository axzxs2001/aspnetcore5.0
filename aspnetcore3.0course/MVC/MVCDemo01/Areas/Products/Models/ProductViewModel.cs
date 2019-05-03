using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemo01.Areas.Products.Models
{
    public class ProductViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }
    }
}
