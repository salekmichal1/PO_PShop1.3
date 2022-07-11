using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public partial class Product
    {
        //public Product()
        //{
        //    OrderedProducts = new HashSet<OrderedProduct>();
        //}
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
        public int ProducerId { get; set; }
        public short AvailablePieces { get; set; }
        public decimal NetCatalogPrice { get; set; }
        public decimal NetSellingPrice { get; set; }
        public decimal NetSellingWarehouse { get; set; }
        public byte VatRate { get; set; }

        public List<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();

        //public virtual ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
