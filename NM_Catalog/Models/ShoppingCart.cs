using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NM_Catalog.Models
{
    [Table("ShoppingCarts", Schema = "catalog")]
    public class ShoppingCart
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}