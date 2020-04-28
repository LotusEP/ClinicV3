
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

        public ActionResult DeleteClinic(int ID)
        {
            clinicModel.DeleteClinic(ID);

            return RedirectToAction("listofClinic");

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
            return RedirectToAction("listofClinic");
        }
        [HttpGet]
        public ActionResult EditClinic(int id)
        {
            clinicModel clinic = clinicModel.GetClinic(id);
            return View(clinic);
        }

        [HttpPost]
        public ActionResult EditClinic(clinicModel clinic)
        {
            clinicModel.EditClinic(clinic);
            ViewBag.editMess = "Success";
            return View();
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
            NewCriteria.listofClinic = clinicModel.GetClinicList("No Password");
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
            NewCriteria.listofClinic = clinicModel.GetClinicList("No Password");
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
        [HttpGet]
        public ActionResult EditReq(int ID)
        {
            Criteria updateCriteria = Criteria.GetInCriteria(ID);
            return View(updateCriteria);
        }
        [HttpPost]
        public ActionResult EditReq(Criteria criteria)
        {
        
            Criteria.EditCriteria(criteria);
                ViewBag.editMess = "Success";
            return View();
        }
           

        public ActionResult AdminOptions()
        {
            ViewBag.Message = "Admin Options page.";
            return View();
        }

       
        public ActionResult AdminLogin()
            {
               
             
            return View(); 

            }

        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult listofClinic()
        {
    
            List<clinicModel> listofClinic = clinicModel.GetClinicList("Password");


            ViewBag.Collection = listofClinic;
            return View(listofClinic);
        }


        public ActionResult Testview()
        {

            DataViewModel DataInfo = new DataViewModel();
            DataInfo.TrafficInfo = DataModel.Source();

            return View(DataInfo);
        }
        [HttpGet]
        public ActionResult EditSmtp(int ID)
        {
            SmtpModel editModel = SmtpModel.GetStmp(ID);
            return View(editModel);
        }
        [HttpPost]
        public ActionResult EditSmtp(SmtpModel stmp)
        {
            SmtpModel.EditStmp(stmp);
            ViewBag.mess = "Success";
            return View();
        }
        [HttpGet]
        public ActionResult AddSmtp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSmtp(SmtpModel stmp)
        {
            SmtpModel.AddStmp(stmp);
            ViewBag.mess = "Success";
            return View();
        }

        public ActionResult DeleteSmtp(int ID)
        {

            SmtpModel.DeleteStmp(ID);
            return RedirectToAction("ListSmtp");
        }
        public ActionResult ListSmtp()
        {
            List<SmtpModel> newlist = SmtpModel.getSmtpList();

            return View(newlist);
            
        }

        public ActionResult DetailPage(int ID)
        {

            clinicModel clinic = clinicModel.GetClinic(ID);
            return View(clinic);
        }
    }
}