using System.Collections.Generic;
using System.Linq;

namespace first_exam.Models
{
    public class Repository : IRepository
    {
        private Dictionary<string, Product> products;
        public Repository()
        {
            products = new Dictionary<string, Product>();

            products.Add("1", new Product { Name = "Women Shoes", Price = 99M });

            products.Add("2", new Product { Name = "Glasses", Price = 49.99M });

            products.Add("3", new Product { Name = "Pants", Price = 40.5M });
        }

        public List<Product> Products()
        {
            return products.Values.ToList();
        }
    }

    
}