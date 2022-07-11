using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public partial class Customer
    {
        //public Customer()
        //{
        //    Orders = new HashSet<Order>();
        //}
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? CompanyName { get; set; }
        public int? CompanyNumber { get; set; }
        public string Street { get; set; } = null!;
        public int StreetNumber { get; set; }
        public int? FlatNumber { get; set; }
        public string PostCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Mail { get; set; } = null!;

        public List<Order> Orders { get; } = new List<Order>();

        //public virtual ICollection<Order> Orders { get; set; }
    }
}
