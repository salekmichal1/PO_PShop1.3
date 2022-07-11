using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public partial class Employee
    {
        //public Employee()
        //{
        //    Orders = new HashSet<Order>();
        //}
        [Key]
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string EmployeeSurname { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public decimal? EmployeeBonus { get; set; }

        public List<Order> Orders { get; } = new List<Order>();

        //public virtual ICollection<Order> Orders { get; set; }
    }
}
