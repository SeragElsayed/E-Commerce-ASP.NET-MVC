using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EComm.Web.Controllers
{
    public class HomeController : Controller
    {
        ECommDB DB;
        public HomeController()
        {
            DB = new ECommDB();
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            User user = new User();
            return PartialView(user);
        }
        
        [HttpGet]
        public ActionResult SignUp()
        {
            User user = new User();
            return PartialView(user);
        }

       

    }
}