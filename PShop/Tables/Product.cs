using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public class Product
    {
        [Key]

        public int id { get; set; }

        public string productName { get; set; }

        public int availablePieces { get; set; }

        public decimal netPrice { get; set; }

        public decimal netSellingPrice { get; set; }

        public List<OrderedProduct> orderedProduct { get; } = new List<OrderedProduct>();
    }
}
