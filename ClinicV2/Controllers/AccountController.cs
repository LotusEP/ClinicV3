using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicV2.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (Session["AdminIsLoggin"] != null)
            {
                return RedirectToAction("AdminOptions", "Info");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "password")
            {
                Session["AdminIsLogin"] = true;
                return RedirectToAction("AdminOptions", "Info");
            }

            ViewBag.Error = "Wrong username/password";
            return View();
        }

        public ActionResult Logout()
        {
            Session["AdminIsLogin"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}