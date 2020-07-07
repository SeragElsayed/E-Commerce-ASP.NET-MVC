using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EComm
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public virtual Product Products { get; set; }
        public virtual User Users { get; set; }

        public Cart()
        {

        }

    }
}