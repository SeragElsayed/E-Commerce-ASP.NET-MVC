using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name="Product Name")]
        [Required]
        //[Remote("CheckProductName","Product",ErrorMessage ="This product already exists")]
        [Remote("CheckProductName", "Product", AdditionalFields = "WhatView", ErrorMessage = "This Product already exists")]

        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        [Required]

        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name="Hide")]
        public bool IsShowen { get; set; }

        public virtual Category Category { get; set; }

        [Required()]
        public  int CategoryId { get; set; }


        public virtual ICollection<Order> Orders { get; set; }

        [Display(Name = "Product Image")]
        [DataType(DataType.ImageUrl)]
        public string ImageURL { get; set; }
        public Product()
        {
            this.Orders = new HashSet<Order>();
            IsShowen = true;
            //ImageURL = $"http://localhost:portnumber/ProductImages/{Name}.jpg";
        }

    }
}