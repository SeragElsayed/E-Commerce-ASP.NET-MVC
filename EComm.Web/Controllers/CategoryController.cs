using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm.Web.Controllers
{
    public class CategoryController : Controller
    {
        ECommDB DB;
        public CategoryController()
        {
            DB = new ECommDB();
        }


        public ActionResult GetAllCategories()
        {
            if (Session["IsAdmin"] != null)
            {

            var Categories = DB.Categories.ToList();
            return PartialView(Categories);
            }
            else
            {
                return Content("not Auth.");
            }
        }
        // GET: Category
        public ActionResult Index()
        {
            if (Session["IsAdmin"] != null)
            {

                return View();
            }
            else
            {
                return Content("not Auth.");
            }

            
        }


        // GET: Category/Create
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["IsAdmin"] != null)
            {

                return PartialView();
            }
            else
            {
                return Content("not Auth.");
            }
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category NewCategory)
        {
            try
            {
                if (Session["IsAdmin"] != null)
                {

                    // TODO: Add insert logic here

                    DB.Entry(NewCategory).State = System.Data.Entity.EntityState.Added;
                    DB.SaveChanges();
                    return PartialView("GetAllCategories", DB.Categories);
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

        // GET: Category/Edit/5
        public ActionResult Edit(int Id)
        {
            if (Session["IsAdmin"] != null)
            {

                var Category = DB.Categories.SingleOrDefault(c => c.Id == Id);
                return PartialView(Category);
            }
            else
            {
                return Content("not Auth.");
            }
            
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category EditedCategory)
        {
            try
            {
                if (Session["IsAdmin"] != null)
                {

                    // TODO: Add update logic here
                    DB.Entry(EditedCategory).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();
                    return PartialView("GetAllCategories", DB.Categories);
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

        // GET: Category/Delete/5
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (Session["IsAdmin"] != null)
            {
                DB.Categories.Remove(DB.Categories.SingleOrDefault(c => c.Id == Id));
                DB.SaveChanges();
                return PartialView("GetAllCategories", DB.Categories);
            }
            else
            {
                return Content("not Auth.");
            }
           
        }

      
        public ActionResult CheckCategoryName(string Name, string WhatView)
        {

            return Json(IsUnique(Name, WhatView), JsonRequestBehavior.AllowGet);

        }


        private bool IsUnique(string Name, string WhatView)
        {
            if (WhatView == "Edit") // its a new object
            {
                var c = DB.Categories.Count(p => (p.Name.ToLower() == Name.ToLower()));
                if (c > 1)
                    return false;
                else
                    return true;
            }
            else
            {
                bool Exists = DB.Categories.Any(p => p.Name.ToLower() == Name.ToLower());
                return !Exists;
            }
        }

    }
}
