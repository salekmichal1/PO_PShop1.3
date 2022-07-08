using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public class Order
    {
        [Key]

        public int id { get; set; }

        public DateTime dateOfPlacingTheOrder { get; set; }

        public bool whetherTheOrderFulfilled { get; set; }

        public int customerId { get; set; }

        public int employeeId { get; set; }

        public List<OrderedProduct> orderedProduct { get; } = new List<OrderedProduct>();

        public Customer customer { get; set; }

        public Employee employee { get; set; }
    }
}
