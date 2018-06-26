using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NM_Catalog.Models
{
    [Table("Products", Schema = "catalog")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("sub_category_id")]
        public int SubCategoryId { get; set; }

        [Column("product_image")]
        public string ProductImage { get; set; }

        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}