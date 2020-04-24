using clinic.Models;

using System;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using ClinicV2.Models;
using MySql.Data.MySqlClient;

namespace ClinicV2.Controllers
{

    //<a href="https://localhost:44330/Info/test">Medi</a>
    public class InfoController : Controller
    {
        // GET: Info
        public ActionResult Index()
        {
            String Ref = Request.Headers["Referer"];

            return View();
        }

        //2nd version of email ---------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Signup()
        {
            String Ref = Request.Headers["Referer"];

            List<clinicModel> listofClinic = clinicModel.GetClinicList();


            ViewBag.Collection = listofClinic;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupModel newSignee, string [] clinicName)
        {
            PageRedirct("Submittion");
            String Ref = Request.Headers["Referer"];

            //foreach (var mail in clinicT)
            //{
            //    if (mail)
            //}
            //string result = collection["clinicT"];

            Patient patient = newSignee.newPatient;
            string message = "Patient Email: " + patient.Email + "\n" + "Patient Phone Number: " + patient.CellPhone + "\n" + "Patient Address: " + patient.Street
           + "\n" + patient.City + "\n" + patient.State + "\n" + patient.Zip;


            
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var mess = new MailMessage();
            //        var senderEmail = new MailAddress("Baon23@outlook.com", "Bao");
            //        var receiverEmail = new MailAddress(patient.Email, "Receiver");
            //        var password = "RandomPass";
            //        var sub = "New Patient";
            //        var body = message;
            //        var smtp = new SmtpClient
            //        {
            //            Host = "smtp-mail.outlook.com",
            //            Port = 587,
            //            EnableSsl = true,
            //            DeliveryMethod = SmtpDeliveryMethod.Network,
            //            UseDefaultCredentials = false,
            //            Credentials = new NetworkCredential(senderEmail.Address, password)
            //        };

            //        mess.From = senderEmail;
            //        mess.To.Add(receiverEmail);
            //        //for (int i = 0; i < clinicName.Length; i++)
            //        //{
            //        //    if (clinicName != null)
            //        //    {
            //        //        mess.Bcc.Add(clinicName[i]);
            //        //    }

            //        //}
            //        mess.Subject = sub;
            //        mess.Body = message;



            //        using (smtp)
            //        {
            //            smtp.Send(mess);
            //        }

                    Patient.AddPatient(patient);


                return RedirectToAction("test");
            //    }
            //}
            //catch (Exception)
            //{
            //    ViewBag.Error = "Some Error";
            //}
            //return RedirectToAction("test");

        }
        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult listofClinic()
        {
            PageRedirct("Clinic list");
            List<clinicModel> listofClinic = clinicModel.GetClinicList();


            ViewBag.Collection = listofClinic;
            return View(listofClinic);
        }
        [Filters.AuthorizeAdmin]
        public ActionResult DeleteClinic(string name)
        {
            clinicModel.deleteClinic(name);

            return RedirectToAction("listofCLinic");

        }

        public ActionResult AboutPage()
        {

            return View();
        }

        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult ClinicAdd()
        {
            return View();
        }
        [Filters.AuthorizeAdmin]
        [HttpPost]
        public ActionResult ClinicAdd(clinicModel clinic)
        {
            clinicModel.CreateClinic(clinic);
            return RedirectToAction("listofCLinic");
        }
        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult GetRequirement()
        {
            List<Criteria>Req = Criteria.GetReqList("-10");
       
            return View(Req);
        }
        [Filters.AuthorizeAdmin]
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
        [Filters.AuthorizeAdmin]
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
   
        public ActionResult test()
        {
            PageRedirct("Signup");

            SignupModel tModel = new SignupModel();
            tModel.listofClinic = clinicModel.GetClinicList();
            tModel.listofCriteria = Criteria.GetReqList("old");
            tModel.listofInsurance = Criteria.GetSpecificCriteira("Insurance");
            tModel.GuidelineValue = Criteria.GetCriteria("200% Guidelines"); 
            tModel.newPatient = new Patient();


            return View(tModel);
        }
        //[HttpGet]
        //public ActionResult EditCriteria(int id)
        //{

        //    Criteria criteria = new Criteria();
        //    criteria = Criteria.GetCriteria(id);


        //    return View(criteria);
        //}
        //[HttpPost]
        //public ActionResult EditCriteria(Criteria req)
        //{

        //    Criteria.AddCriteria(req);

        //    return RedirectToAction("ViewReq");
        //}
        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult DeleteCriteria(int id)
        {
   
            Criteria.DeleteCriteria(id,"ClinicCriteria");

            return RedirectToAction("ViewReq");
        }
        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult DeleteCriteriaComplete(int id)
        {
          
            Criteria.DeleteCriteriaComplete(id);

            return RedirectToAction("ViewReq");
        }
        [Filters.AuthorizeAdmin]
        [HttpGet]
        public ActionResult CreateReq()
        {
            CreateCriteriaModel NewCriteria = new CreateCriteriaModel();
            NewCriteria.CriteriaOption = Criteria.CriteraiValue();
            NewCriteria.Criteria = new Criteria();
            NewCriteria.listofCriteriaValue = Criteria.GetCriteriaValue();
            return View(NewCriteria);
        }
        [Filters.AuthorizeAdmin]
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
        public ActionResult Testview()
        {

            DataViewModel DataInfo = new DataViewModel();
            DataInfo.TrafficInfo = DataModel.Source();
           
            return View(DataInfo);
        }


        public void PageRedirct(string PageName)
        {
            String Ref = "Direct";

            Ref = Request.Headers["Referer"];
          
            DateTime localtime = DateTime.Now;

            string Time = localtime.ToString("dd MMMM yyyy hh:mm:ss tt");

            //----------------------------
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);


            //check for exisitng criteria
            string sql;


            sql =
           " Insert Into Traffic (TrafficSource,TimeStamp,Page) Values ('" + Ref + "','" + Time + "','" + PageName + "');";

            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            cmm.ExecuteNonQuery();



            //----------------------------

        }

        [Filters.AuthorizeAdmin]
        public ActionResult AdminOptions()
        {
            ViewBag.Message = "Admin Options page.";
            return View();
        }

        public ActionResult Logout()
        {
            ViewBag.Message = "Logout";
            return RedirectToAction("Index", "Info");
        }


    }
}