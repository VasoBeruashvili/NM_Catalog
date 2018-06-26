using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NM_Catalog.Models
{
    [Table("Categories", Schema = "catalog")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("fontawesome_icon")]
        public string FontawesomeIcon { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}