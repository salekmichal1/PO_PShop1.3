using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public class Employee
    {
        [Key]

        public int Id { get; set; }

        public string employeeName { get; set; }

        public string employeeSurname { get; set; }

        public string login { get; set; }

        public string password { get; set; }

        public decimal? employeeBonus { get; set; }

        public List<Order> order { get; } = new List<Order>();

    }
}
