using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm.Web.Controllers
{
    public class OrderController : Controller
    {
        ECommDB DB;
        public OrderController()
        {
            DB = new ECommDB();
        }
        public ActionResult GetAllOrdersAdmin()
        {
            if (Session["IsAdmin"] != null)
            {
                var Orders = DB.Orders.ToList();
                return PartialView(Orders);
            }
            else
            {
                return Content("not Auth.");
            }
        }
        public ActionResult GetAllOrdersUser(int Id)
        {
            if (Id.ToString() == Session["UserId"].ToString())
            {

            var Orders = DB.Orders.Where(order=>(order.Users.Id==Id && order.Status!=OrderStatus.Cancelled));
            return PartialView(Orders);
            }
            else
            {
                return Content("not Auth.");
            }
        }
        public ActionResult Index(int Id)
        {
            if ( Session["UserId"]!=null && Id == int.Parse(Session["UserId"].ToString()))
            {
                ViewBag.UserId = Session["UserId"].ToString();
                return View();
            }
            else
            {
                return Content("not Auth.");
            }

        }



        //// GET: Order/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult Create(Order order)
        {
            try
            {
                //Order order = new Order();
                int UserId = int.Parse( Session["UserId"].ToString());
                var CurrentUser = DB.Users.SingleOrDefault(u => u.Id == UserId);
                var CurrentUserCart = DB.Carts.Where(c => c.UserId == UserId);
                if (CurrentUserCart!=null )
                {
                    order.Users = CurrentUser;
                    //decimal OrderSum = 0;
                    foreach (var item in CurrentUserCart)
                    {
                        order.Sum+=(int)item.Products.Price;
                        order.Products.Add(item.Products);
                    }
                    
                    // TODO: Add insert logic here
                    DB.Entry(order).State = System.Data.Entity.EntityState.Added;
                    DB.SaveChanges();
                    
                    return RedirectToAction("ClearCart","Cart");

                }
                else
                {
                    return Content("not Auth.");

                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int Id,string AdminAction)
        {
            var order = DB.Orders.SingleOrDefault(o => o.Id == Id && o.Status==OrderStatus.Pending);
            if(order!=null && Session["IsAdmin"]!=null)
            {

                switch (AdminAction)
                {
                    case "Accept":
                        order.Status = OrderStatus.Accepted;

                        break;
                    case "Reject":
                        order.Status = OrderStatus.Rejected;

                        break;
                    
                    default:
                        break;
                }
                DB.Entry(order).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return JavaScript("location.reload(true)");

            }
            else
            {
                return Content("not Auth.");
            }
        }

      

        [HttpDelete]
        public ActionResult Delete(int Id)
        {
            var order = DB.Orders.Include("Users").SingleOrDefault(o => o.Id == Id && o.Status==OrderStatus.Pending);
            if (Session["UserId"].ToString()==order.Users.Id.ToString())
            {
                order.Status = OrderStatus.Cancelled;
                DB.Entry(order).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return JavaScript("location.reload(true)");

            }
            else
            {
                return Content("not Auth");
            }

                
        }
        public ActionResult OrderDetails(int Id)
        {
            var order = DB.Orders.Include("Users").SingleOrDefault(o => o.Id == Id && o.Status != OrderStatus.Cancelled);
            if (Session["UserId"].ToString() == order.Users.Id.ToString() || Session["IsAdmin"]!=null)
            {
                return PartialView(order);
            }
            else
            {
                return Content("not Auth");
            }
        }


    }
}
