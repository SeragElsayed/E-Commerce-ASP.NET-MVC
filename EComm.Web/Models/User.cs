using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name="User Name")]
        [Remote("CheckUsername", "User", AdditionalFields = "WhatView", ErrorMessage = "This Username already exists")]
        //[Remote("doesCnicExist", "employee", AdditionalFields = "id", HttpMethod = "POST", ErrorMessage = "A user with this cnic already exists. Please enter a different cnic.")]

        public string Username { get; set; }

        [Display(Name="Password")]
        [DataType(DataType.Password)]
        [Required()]
        public string Password { get; set; }

        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Display(Name="Profile Picture")]
        [DataType(DataType.ImageUrl)]

        public string ImageURL { get; set; }


        [ScaffoldColumn(false)]

        public bool IsAdmin { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public User()
        {
            Orders = new HashSet<Order>();
            //ImageURL = $"http://localhost:portnumber/ProductImages/{Username}.jpg";
            IsAdmin = false;
        }
    }
}