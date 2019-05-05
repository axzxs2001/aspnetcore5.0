using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCDemo01.Areas.Products.Models;

namespace MVCDemo01.Areas.Products.Services
{
    public class ProductService : IProductService
    {
        public IEnumerable<ProductViewModel> GetProducts()
        {
            return new List<ProductViewModel> {
                new ProductViewModel { ID=1, Name="产品A",Price=3.5m, Quantity=10 },
                new ProductViewModel { ID=2, Name="产品B",Price=4.5m, Quantity=20 },
                new ProductViewModel { ID=3, Name="产品C",Price=5.5m, Quantity=30 }
            };
        }
    }
}
