using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public class OrderedProduct
    {
        [Key]

        public int id { get; set; }

        public int orderId { get; set; }

        public int productId { get; set; }

        public int quantity { get; set; }

        public Product product { get; set; }

        public Order order { get; set; }

    }
}
