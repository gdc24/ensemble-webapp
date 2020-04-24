using ensemble_webapp.Database;
using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ensemble_webapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LoginUser(Users logInUser)
        {
            if (Login.VerifyUser(logInUser.StrUsername, logInUser.StrPassword) {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}