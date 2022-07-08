using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public class Customer
    {
        [Key]

        public int id { get; set; }

        public string customerName { get; set; }

        public string customerSurname { get; set; }

        public string? companyName { get; set; }

        public int? companyNumber { get; set; }

        public string street { get; set; }

        public int streetNumber { get; set; }

        public int? flatNumber { get; set; }

        public string postCode { get; set; }

        public string city { get; set; }

        public string phoneNumber { get; set; }

        public string mail { get; set; }

        public List<Order> order { get; } = new List<Order>();
    }
}
