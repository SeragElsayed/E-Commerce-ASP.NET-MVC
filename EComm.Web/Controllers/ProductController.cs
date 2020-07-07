using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm.Web.Controllers
{
    public class ProductController : Controller
    {
        ECommDB DB;
        public ProductController()
        {
            DB = new ECommDB();
        }
        public ActionResult GetAllProductsAdmin()
        {
            if (Session["IsAdmin"] != null)
            {

                var Products = DB.Products.ToList();
                return PartialView(Products);
            }
            else
                return Content("not Auth.");
        }
        public ActionResult GetAllProductsUser()
        {
            if (Session["UserId"] != null)
            {
                var Products = DB.Products.Include("Category").ToList();
                return PartialView(Products);
            }
            else
                return Content("not Auth.");
        }
        public ActionResult GetProductsUserByCategory(int CategoryId)
        {
            if (Session["UserId"] != null)
            {
                
                var Products = DB.Products.Where(p=>p.CategoryId==CategoryId).ToList();
                return PartialView(Products);
            }
            else
                return Content("not Auth.");
        }
        public ActionResult Index()
        {

            if (Session["UserId"] != null)
            {
                var categories = DB.Categories.ToList();
                return View(categories);

            }
            else
                return Content("not Auth.");
        }
        //public ActionResult CheckProductName(string Name)
        //{
        //    bool Exists = DB.Products.Any(p => p.Name.ToLower() == Name.ToLower());
        //    return  Json(!Exists, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult CheckProductName(string Name, string WhatView)
        {

            return Json(IsUnique(Name, WhatView), JsonRequestBehavior.AllowGet);

        }


        private bool IsUnique(string Name, string WhatView)
        {
            if (WhatView != "") // its a new object
            {
                var c=DB.Products.Where(p => (p.Name.ToLower() == Name.ToLower()));
                if (c.Count() > 1)
                    return false;

                if (c.Count() == 1 && c.First().Id.ToString() != WhatView)
                    return false;

                else
                    return true;
            }
            else
            {
                bool Exists = DB.Products.Any(p => p.Name.ToLower() == Name.ToLower());
                return !Exists;
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["IsAdmin"] != null)
            {
                var Categories = DB.Categories.ToList();
                var CategoriesSelectList = new SelectList(Categories, "Id", "Name", 1);
                ViewBag.Categories = CategoriesSelectList;
                return PartialView();
            }
            else
                return Content("not Auth.");
            
        }
        [HttpPost]
        public ActionResult Create(Product NewProduct, HttpPostedFileBase ProductImage)
        {
            if (Session["IsAdmin"] != null)
            {

                if (ProductImage != null)
                {
                    string ImgExt = Path.GetExtension(ProductImage.FileName);

                    //if (ImgExt.ToLower() != ".jpg" || ImgExt.ToLower() != ".jpeg")
                    //    return Content("notValid File Extension");

                    ProductImage.SaveAs(Server.MapPath($"~/ProductImage/{NewProduct.Name}{ImgExt}"));

                    NewProduct.ImageURL = $"~/ProductImage/{NewProduct.Name}{ImgExt}";

                }
                NewProduct.ImageURL = $"~/ProductImage/default.jpg";

                DB.Entry(NewProduct).State = System.Data.Entity.EntityState.Added;
                DB.SaveChanges();
                return PartialView("GetAllProductsAdmin", DB.Products);
            }
            else
                return Content("not Auth.");

            

        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            if (Session["IsAdmin"] != null)
            {
                var Product = DB.Products.SingleOrDefault(p => p.Id == Id);
                var Categories = DB.Categories.ToList();
                var CategoriesSelectList = new SelectList(Categories, "Id", "Name", Product.CategoryId);
                ViewBag.Categories = CategoriesSelectList;
                return PartialView(Product); ;
            }
            else
                return Content("not Auth.");
            
        }

        void DeleteProfileOldImage(string ImagePath)
        {
            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }
        }


        [HttpPost]
        public ActionResult Edit(Product EditedProduct, HttpPostedFileBase ProductImage)
        {
            if (Session["IsAdmin"] != null)
            {

                if (ProductImage != null)
                {
                    //var ou = DB.Users.SingleOrDefault(u => u.Id == EditedProduct.Id);
                    if (EditedProduct.ImageURL != "~/ProductImage/default.jpg")
                        DeleteProfileOldImage(Request.MapPath(EditedProduct.ImageURL));

                    string ImgExt = Path.GetExtension(ProductImage.FileName);

                    //if (ImgExt.ToLower() != ".jpg" || ImgExt.ToLower() != ".jpeg")
                    //    return Content("notValid File Extension");

                    ProductImage.SaveAs(Server.MapPath($"~/ProductImage/{EditedProduct.Name}{ImgExt}"));


                    EditedProduct.ImageURL = $"~/ProductImage/{EditedProduct.Name}{ImgExt}";

                }

                DB.Entry(EditedProduct).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return PartialView("GetAllProductsAdmin", DB.Products);
            }
            else
                return Content("not Auth.");
            

        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session["IsAdmin"] != null)
            {
                DB.Products.Remove(DB.Products.SingleOrDefault(p => p.Id == Id));
                DB.SaveChanges();
                return PartialView("GetAllProductsAdmin", DB.Products);
            }
            else
                return Content("not Auth.");
            
        }
        
        [HttpGet]
        public ActionResult ProductDetails(int Id)
        {
            if (Session["UserId"] != null)
            {
                var product = DB.Products.Include("Category").SingleOrDefault(p => p.Id == Id);
                return PartialView(product);
            }
            else
            {
                return Content("not Auth.");
            }
        }

    }
}