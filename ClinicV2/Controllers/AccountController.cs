using ClinicV2.Models;
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
           
            if (Session["AdminIsLogin"] != null)
            {
                HttpCookie cookie = Request.Cookies["userInfo"];

                return RedirectToAction("AdminOptions", "AdminOptions");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Account newAccount = new Account(username, password);
            bool checkCounter = Account.login(newAccount);
            if (checkCounter != false)
            {
                Session["AdminIsLogin"] = true;
                DateTime dateStamp = DateTime.Now;
                HttpCookie cookie = new HttpCookie("userInfo",username);
                Response.Cookies.Add(cookie);
                return RedirectToAction("AdminOptions", "AdminOptions");
            }

            ViewBag.Error = "Wrong username/password";
            return View();
        }

        public ActionResult Logout()
        {
            Session["AdminIsLogin"] = null;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult CreatAccount()
        {
        
            return View();
        }
        [HttpPost]
        public ActionResult CreatAccount(Account account)
        {
            Account.Create(account);
            return View();
        }
    }
}