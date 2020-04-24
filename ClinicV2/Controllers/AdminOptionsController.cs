
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using ClinicV2.Models;
using System.Web.Mvc;
using clinic.Models;
using System.Activities;

namespace ClinicV2.Controllers
{
    [Filters.AuthorizeAdmin]
    public class AdminOptionsController : Controller
    {

        public ActionResult DeleteClinic(string name)
        {
            clinicModel.deleteClinic(name);

            return RedirectToAction("listofCLinic");

        }

        [HttpGet]
        public ActionResult ClinicAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClinicAdd(clinicModel clinic)
        {
            clinicModel.CreateClinic(clinic);
            return RedirectToAction("listofCLinic");
        }
 
        [HttpGet]
        public ActionResult GetRequirement()
        {
            List<Criteria>Req = Criteria.GetReqList("-10");
       
            return View(Req);
        }
 
        [HttpGet]
        public ActionResult ViewReq()
        {

            CreateCriteriaModel NewCriteria = new CreateCriteriaModel();
            NewCriteria.listofClinic = clinicModel.GetClinicList();
            NewCriteria.listofCriteria = Criteria.GetReqList("-10");
            NewCriteria.CriteriaOption = Criteria.CriteraiValue();
            NewCriteria.Criteria = new Criteria();
            NewCriteria.listofCriteriaValue = Criteria.GetCriteriaValue();
            return View(NewCriteria);
        }
  
        [HttpPost]
        public ActionResult ViewReq(CreateCriteriaModel req)
        {
 
            ViewBag.ExistMess = Criteria.AddCriteria(req.Criteria,"ClinicCriteria");
            CreateCriteriaModel NewCriteria = new CreateCriteriaModel();
            NewCriteria.listofClinic = clinicModel.GetClinicList();
            NewCriteria.listofCriteria = Criteria.GetReqList("-10");
            NewCriteria.CriteriaOption = Criteria.CriteraiValue();
            NewCriteria.Criteria = new Criteria();
            NewCriteria.listofCriteriaValue = Criteria.GetCriteriaValue();
            return View(NewCriteria);

        }

        [HttpGet]
        public ActionResult DeleteCriteria(int id)
        {
   
            Criteria.DeleteCriteria(id,"ClinicCriteria");

            return RedirectToAction("ViewReq");
        }

        [HttpGet]
        public ActionResult DeleteCriteriaComplete(int id)
        {
          
            Criteria.DeleteCriteriaComplete(id);

            return RedirectToAction("ViewReq");
        }

        [HttpGet]
        public ActionResult CreateReq()
        {
            CreateCriteriaModel NewCriteria = new CreateCriteriaModel();
            NewCriteria.CriteriaOption = Criteria.CriteraiValue();
            NewCriteria.Criteria = new Criteria();
            NewCriteria.listofCriteriaValue = Criteria.GetCriteriaValue();
            return View(NewCriteria);
        }

        [HttpPost]
        public ActionResult CreateReq(CreateCriteriaModel req)
        {
            ViewBag.ExistMess = Criteria.AddCriteria(req.Criteria, "CriteriaOption");
            CreateCriteriaModel NewCriteria = new CreateCriteriaModel();
            NewCriteria.CriteriaOption = Criteria.CriteraiValue();
            NewCriteria.Criteria = new Criteria();
            NewCriteria.listofCriteriaValue = Criteria.GetCriteriaValue();
            return View(NewCriteria);
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

        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult listofClinic()
        {
    
            List<clinicModel> listofClinic = clinicModel.GetClinicList();


            ViewBag.Collection = listofClinic;
            return View(listofClinic);
        }


        public ActionResult Testview()
        {

            DataViewModel DataInfo = new DataViewModel();
            DataInfo.TrafficInfo = DataModel.Source();

            return View(DataInfo);
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