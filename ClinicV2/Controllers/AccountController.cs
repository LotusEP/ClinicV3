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
        [Filters.AuthorizeAdmin]
        public ActionResult CreatAccount()
        {
        
            return View();
        }
        [Filters.AuthorizeAdmin]
        [HttpPost]
        public ActionResult CreatAccount(Account account)
        {
            Account.Create(account);
            return View();
        }
        [Filters.AuthorizeAdmin]
        public ActionResult ListAccount()
        {
            List<Account> newlist = Account.GetAccountList() ;

            return View(newlist);
        }
        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult EditAccount(int ID)
        {
              
            return View(Account.GetAccount(ID));
        }
        [Filters.AuthorizeAdmin]
        [HttpPost]
        public ActionResult EditAccount(Account acc)
        {
            Account.EditAccountInfo(acc);
            ViewBag.mess = "Success";
            return View();
        }
        [Filters.AuthorizeAdmin]
        public ActionResult DeleteAccount(int ID) {
            return View();
        }
    }
}