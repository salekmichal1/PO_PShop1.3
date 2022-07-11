using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public partial class Order
    {
        //public Order()
        //{
        //    OrderedProducts = new HashSet<OrderedProduct>();
        //}
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateOfPlacingTheOrder { get; set; }
        public DateTime? OrderRealizationDate { get; set; }
        public bool? WhetherTheOrderFulfilled { get; set; }
        public DateTime? ShippingDate { get; set; }
        public int EmployeeId { get; set; }
        public int InvoiceId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public List<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();

        //public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
