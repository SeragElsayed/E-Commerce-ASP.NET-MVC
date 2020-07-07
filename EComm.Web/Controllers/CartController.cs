using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm.Web.Controllers
{
    public class CartController : Controller
    {
        ECommDB DB;
        public CartController()
        {
            DB = new ECommDB();
        }
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                int UserId= int.Parse(Session["UserId"].ToString());
                var cart = DB.Carts.Include("Products").Where(c => c.UserId ==UserId ).ToList();
                return View(cart);

            }
            else
            {
                return Content("not Auth.");
            }

        }
        public ActionResult CartItemNumber()
        {
            int userId = int.Parse(Session["UserId"].ToString());
            var CartItemNumber = DB.Carts.Count(c => c.UserId == userId);
            return Content($"<span  id='CartItemNumber'>{CartItemNumber}</span>");

        }
        public ActionResult Add(Cart qcart)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            //var currentusercart = DB.Carts.SingleOrDefault(c => c.UserId == userId && c.ProductId==qcart.ProductId );
            var currentusercart = DB.Carts.SingleOrDefault(c => c.UserId == userId && c.ProductId==qcart.ProductId );
            if (currentusercart == null)
            {
                qcart.UserId = userId;
                qcart.ProductQuantity = 1;
                DB.Entry(qcart).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                currentusercart.ProductQuantity++;
                DB.Entry(currentusercart).State = System.Data.Entity.EntityState.Modified;
            }
            DB.SaveChanges();
            var CartItemNumber = DB.Carts.Count(c => c.UserId == userId);

            return Content($"<span  id='CartItemNumber'>{CartItemNumber}</span>");
        }
        public ActionResult IncrementProduct(int Id)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            var currentusercart = DB.Carts.SingleOrDefault(c => (c.UserId == userId && c.Id == Id));
            if (currentusercart != null)
            {
                currentusercart.ProductQuantity++;
                DB.Entry(currentusercart).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return Content($"<td  id=$'{currentusercart.Products.Name}'>{currentusercart.ProductQuantity}</td>");
            }
            else
            {
                return Content("not Auth.");
            }
        }
        public ActionResult DecrementProduct(int Id)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            var currentusercart = DB.Carts.SingleOrDefault(c => c.UserId == userId && c.Id == Id);
            if (currentusercart != null)
            {
                if(currentusercart.ProductQuantity > 1)
                {
                    currentusercart.ProductQuantity--;
                    DB.Entry(currentusercart).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                return Content($"<td  id=$'{currentusercart.Products.Name}'>{currentusercart.ProductQuantity}</td>");
                }
                else
                {
                    DeleteProduct(Id);
                    return JavaScript("location.reload(true)");
                }

            }
            else
            {
                return Content("not Auth.");
            }
        }
        public ActionResult DeleteProduct(int Id)
        {
            int userId = int.Parse(Session["UserId"].ToString());
            var currentusercart = DB.Carts.SingleOrDefault(c => c.UserId == userId && c.Id == Id);
            if (currentusercart != null)
            {
                DB.Entry(currentusercart).State = System.Data.Entity.EntityState.Deleted;
                DB.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Content("not Auth.");
            }
        }
        public ActionResult ClearCart()
        {
            int userId = int.Parse(Session["UserId"].ToString());
            var CurrentUserCart = DB.Carts.Where(c => c.UserId == userId).ToList();
            DB.Carts.RemoveRange(CurrentUserCart);
            DB.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}