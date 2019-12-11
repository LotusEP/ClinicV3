
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


using System.Web.Mvc;
using clinic.Models;
using System.Activities;

namespace ClinicV2.Controllers
{
    public class AdminOptionsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddClinics()
        {
            ViewBag.Message = "Add a New Clinic page.";
            return View();
        }

        public ActionResult EditClinics()
        {
            ViewBag.Message = "Edit Clinics page.";
            return View();
        }

        public ActionResult DeleteClinics()
        {
            ViewBag.Message = "Delete Clinics page.";
            return View();
        }

        public ActionResult ViewDemographics()
        {
            ViewBag.Message = "View Demographics page.";
            return View();
        }


        public ActionResult ClinicAdded()
        {
            ViewBag.Message = "Clinic added";
            return View();
        }

         public ActionResult FindClinics()
            {
                ViewBag.Message = "Find Clinics page.";

                return View();
            }

            
            public ActionResult AdminOptions()
            {
                ViewBag.Message = "Admin Options page.";
                return View();
            }

            public ActionResult AdminLogin()
            {
               

            //Connection to database (Login credentials retrieval at a later date)
             
            return View(); 

            }

        public ActionResult Logout()
        {
            ViewBag.Message = "Logout";
            return RedirectToAction("Index","Info");
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //    public IActionResult Error()
        //    {
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
    
        //[responsecache(duration = 0, location = responsecachelocation.none, nostore = true)]
        //public iactionresult error()
        //{
        //    return view(new errorviewmodel { requestid = activity.current?.id ?? httpcontext.traceidentifier });
        //}
    }
}