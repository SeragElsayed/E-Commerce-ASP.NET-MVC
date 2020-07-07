using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm.Web.Controllers
{
    public class UserController : Controller
    {
        ECommDB DB;
        public UserController()
        {
            DB = new ECommDB();
        }

       
        public ActionResult Profile(int Id)
        {
            if (Session["UserId"] != null && Session["UserId"].ToString() == Id.ToString())
            {
                var user = DB.Users.SingleOrDefault(u => u.Id == Id);
                return View("LogIn", user);
            }
            else
                return Content("not Auth.");
        }

        [HttpPost]
        public ActionResult LogIn(User user)
        {
            var UserDB = DB.Users.SingleOrDefault(u => u.Username == user.Username);
            if (UserDB == null)
                return Content("not found");

            if (user.Password == UserDB.Password)
            {
                Session["UserId"] = UserDB.Id.ToString();
                if (UserDB.IsAdmin==true)
                {
                    Session["IsAdmin"] = "Admin";
                }
                
                return View(UserDB);

            }

            else
                return Content("not found");
        }


        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User NewUser,HttpPostedFileBase ProfileImage)
        {
            try
            {
                // TODO: Add insert logic here
                NewUser.IsAdmin = false;
                if (ProfileImage != null)
                {
                    string ImgExt = Path.GetExtension(ProfileImage.FileName);

                    //if (ImgExt.ToLower() != ".jpg" || ImgExt.ToLower() != ".jpeg")
                    //    return Content("notValid File Extension");

                    ProfileImage.SaveAs(Server.MapPath($"~/ProfileImage/{NewUser.Username}{ImgExt}"));
                
                    NewUser.ImageURL = $"~/ProfileImage/{NewUser.Username}{ImgExt}";

                }
                NewUser.ImageURL = $"~/ProfileImage/default.jpg";
                DB.Entry(NewUser).State = System.Data.Entity.EntityState.Added;
                DB.SaveChanges();
                Session["UserId"] = NewUser.Id.ToString();
                //if (NewUser.IsAdmin == true)
                //{
                //    Session["IsAdmin"] = "Admin";
                //}

                return View("LogIn",NewUser);
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            if (Session["UserId"].ToString() == Id.ToString())
            {


                var user = DB.Users.SingleOrDefault(u => u.Id == Id);
                return PartialView(user);
            }
            else
                return Content ("not Auth");
        }


        void DeleteProfileOldImage(string ImagePath)
        {
            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit( User EditedUser, HttpPostedFileBase ProfileImage)
        {
            try
            {
                if (Session["UserId"].ToString() == EditedUser.Id.ToString())
                {
                    if (ProfileImage != null)
                    {
                        //var ou = DB.Users.SingleOrDefault(u => u.Id == EditedUser.Id);
                        if (EditedUser.ImageURL != "~/ProfileImage/default.jpg")
                            DeleteProfileOldImage(Request.MapPath(EditedUser.ImageURL));

                        string ImgExt = Path.GetExtension(ProfileImage.FileName);

                        //if (ImgExt.ToLower() != ".jpg" || ImgExt.ToLower() != ".jpeg")
                        //    return Content("notValid File Extension");

                        ProfileImage.SaveAs(Server.MapPath($"~/ProfileImage/{EditedUser.Username}{ImgExt}"));


                        EditedUser.ImageURL = $"~/ProfileImage/{EditedUser.Username}{ImgExt}";

                    }
                    // TODO: Add update logic here
                    //DB.Users.Attach(EditedUser);
                    DB.Entry(EditedUser).State = System.Data.Entity.EntityState.Modified;
                    DB.SaveChanges();

                    return RedirectToAction("Profile",new { Id = EditedUser.Id });
                }
                else
                    return Content("not Auth");
                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            Session["IsAdmin"] = null;
            return RedirectToAction("Index", "Home");

        }
        public ActionResult CheckUsername(string Username,string WhatView)
               {
            
            return Json(IsUnique(Username, WhatView), JsonRequestBehavior.AllowGet);

        }
       

        private bool IsUnique(string Username, string WhatView)
        {
            if (WhatView == "Edit" ) // its a new object
            {
                var c = DB.Users.Where(u => (u.Username.ToLower() == Username.ToLower()));
                if (c.Count() > 1)
                    return false;

                else if (c.Count() == 1)
                {
                    if (Session["UserID"] != null && c.First().Id != int.Parse(Session["UserId"].ToString()))
                        return false;
                  
                    return true;
                }

                else
                    return true;
            }
            else 
            {
                return !DB.Users.Any(u => u.Username.ToLower() == Username.ToLower());
            }
           
        }

    }
}
