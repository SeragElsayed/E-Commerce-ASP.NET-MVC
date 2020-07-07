using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EComm
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required()]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required()]
        [DataType(DataType.Currency)]
        [Display(Name="Total Order Sum")]
        public decimal Sum { get; set; }

        [Required()]
        [Display(Name="Order Status")]
        public OrderStatus Status { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual User Users { get; set; }
        public Order()
        {
            this.Products = new HashSet<Product>();
            Status = OrderStatus.Pending;
            Date = DateTime.Now;
           
        }
    }
}