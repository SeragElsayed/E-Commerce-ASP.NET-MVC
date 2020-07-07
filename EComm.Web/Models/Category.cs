using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EComm
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name="Category")]
        [Required()]
        [Remote("CheckCategoryName","Category", AdditionalFields="WhatView")]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}