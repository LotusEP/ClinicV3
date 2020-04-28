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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignupConfirmation(SignupModel newSignee, string [] clinicName,string clinic, string clinicID)
        {
            //keep track where the patitent come from
            if (clinic == null)
            {
                clinic = "Direct";
            }
            PageRedirct(clinic);

  
            //create the patient object
            Patient patient = newSignee.newPatient;
            //message for the email
            string message = "Patient Email: " + patient.Email + "\n" + "Patient Phone Number: " + patient.CellPhone + "\n" + "Patient Address: " + patient.Street
           + "\n" + patient.City + "\n" + patient.State + "\n" + patient.Zip;
            //determine which clinic to send the email
            int ID;
            if (clinicID != null)
            {
                ID = Int32.Parse(clinicID);

            }
            else {
                ID = 1;
            }

            //create and get the send info 
            SmtpModel senderInfo = new SmtpModel();
            senderInfo = SmtpModel.getStmpInfo(ID);
    
            //send the infomation

            try
            {
                if (ModelState.IsValid)
                {

                    var mess = new MailMessage();
                    var senderEmail = new MailAddress(senderInfo.Email, senderInfo.Name);
                    var receiverEmail = new MailAddress(patient.Email, patient.FirstName);
                    var password = senderInfo.MailPassword;
                    var sub = "New Patient";
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = senderInfo.Smtp,
                        Port = senderInfo.Port,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };

                    mess.From = senderEmail;
                    mess.To.Add(receiverEmail);
                    mess.Subject = sub;
                    mess.Body = message;



                    using (smtp)
                    {
                        smtp.Send(mess);
                    }

                    Patient.AddPatient(patient);

                    ViewBag.mess = patient.FirstName;
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return RedirectToAction("Signup");

        }



        public ActionResult AboutPage()
        {

            return View();
        }


        
        public ActionResult Signup(string clinic)
        {
            PageRedirct(clinic);
            //get information for the signup form
            SignupModel tModel = new SignupModel();
            tModel.listofClinic = clinicModel.GetClinicList("No password");
            tModel.listofCriteria = Criteria.GetReqList("old");
            tModel.listofInsurance = Criteria.GetSpecificCriteira("Insurance");
            tModel.GuidelineValue = Criteria.GetCriteria("200% Guidelines"); 
            tModel.newPatient = new Patient();


            return View(tModel);
        }


        //get the source desitnation and store it
        public void PageRedirct(string PageName)
        {
            if (PageName != null)
            {
                     DateTime localtime = DateTime.Now;
                    string sqlFormattedDate = localtime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    string Time = localtime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    //----------------------------
                    string connString;
                    MySqlConnection cnn;
                    connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
                    cnn = new MySqlConnection(connString);


                    //check for exisitng criteria
                    string sql;


                    sql =
                   " Insert Into DestinationSource (SourceName,TimeStamp) Values ('" + PageName + "','" + Time + "');";

                    MySqlCommand cmm = new MySqlCommand(sql, cnn);
                    cnn.Open();
                    cmm.ExecuteNonQuery();



                    //----------------------------


            }
          
            
        }



    }
}