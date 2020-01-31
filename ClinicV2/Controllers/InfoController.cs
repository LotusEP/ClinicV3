using clinic.Models;

using System;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using ClinicV2.Models;

namespace ClinicV2.Controllers
{
    public class InfoController : Controller
    {
        // GET: Info
        public ActionResult Index()
        {
            return View();
        }

        //2nd version of email ---------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Signup()
        {
            List<clinicModel> listofClinic = clinicModel.GetClinicList();


            ViewBag.Collection = listofClinic;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupModel newSignee, string [] clinicName)
        {

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
            //        var password = "Hello123@@";
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
         //       }
         //   }
          //  catch (Exception)
         //   {
          //      ViewBag.Error = "Some Error";
          //  }
         //   return RedirectToAction("test");

        }

        [HttpGet]
        public ActionResult listofClinic()
        {
            List<clinicModel> listofClinic = clinicModel.GetClinicList();


            ViewBag.Collection = listofClinic;
            return View(listofClinic);
        }

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
        public ActionResult CreateReq()
        {

            CreateCriteriaModel NewCriteria = new CreateCriteriaModel();
            NewCriteria.listofClinic = clinicModel.GetClinicList();
            NewCriteria.listofCriteria = Criteria.GetReqList("-10");
            NewCriteria.CriteriaOption = Criteria.CriteraiValue();
            NewCriteria.Criteria = new Criteria();
            return View(NewCriteria);
        }
        [HttpPost]
        public ActionResult CreateReq(CreateCriteriaModel req)
        {

 
            ViewBag.ExistMess = Criteria.AddCriteria(req.Criteria);
            
            return RedirectToAction("CreateReq");
        }
   
        public ActionResult test()
        {
            SignupModel tModel = new SignupModel();
            tModel.listofClinic = clinicModel.GetClinicList();
            tModel.listofCriteria = Criteria.GetReqList("old");
            tModel.newPatient = new Patient();


            return View(tModel);
        }
        [HttpGet]
        public ActionResult EditCriteria(int id)
        {

            Criteria criteria = new Criteria();
            criteria = Criteria.GetCriteria(id);
            

            return View(criteria);
        }
        [HttpPost]
        public ActionResult EditCriteria(Criteria req)
        {

            Criteria.AddCriteria(req);

            return RedirectToAction("CreateReq");
        }
        [HttpGet]
        public ActionResult DeleteCriteria(int id)
        {
            Criteria req = new Criteria();
            req = Criteria.GetCriteria(id);
            Criteria.DeleteCriteria(req);

            return RedirectToAction("CreateReq");
        }


    }
}