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
        public ActionResult SignupConfirmation(SignupModel newSignee, int [] clinicName,string clinic, string clinicID)
        {
            //keep track where the patitent come from
            if (clinic == null)
            {
                clinic = "Direct";
            }
            PageRedirct(clinic);

            List<clinicModel> listOfClin = new List<clinicModel>();

            for (int x = 0; x < clinicName.Length; x++)
            {
                clinicModel toAdd = clinicModel.GetClinic(clinicName[x]);

                if (toAdd != null)
                {
                    listOfClin.Add(toAdd);
                }
            }
            //create the patient object
            Patient patient = newSignee.newPatient;
            //message for the email
            string message ="", message2="";

            message += "This person have match with your clinic requirement, this is their information. They have received your information. \n " +
                "Below is their contact information. \n" +
                "Please contact them in the new few days.";

   

                message += "Name:" + patient.FirstName +" " + patient.LastName + "\n Email: " + patient.Email + "\n  Phone Number: " + patient.CellPhone + "\n" + " Address: " + patient.Street
           + "," + patient.City + "," + patient.State + "," + patient.Zip;


            message2 += "We thank you for using our service, below is the list and information regarding the clinic you have chosen.\n";
            message2 += "----------------------------------------------------------------------------------------------------------------- \n";

            for (int x = 0; x < clinicName.Length; x++)
            {
                message2 += "Clinic: " + listOfClin[x].Name + "\n" +
                    "Clinic Email: " + listOfClin[x].Email + "\n" +
                    "Clinic Phone Number: " + listOfClin[x].PhoneNumber + "\n" +
                    "Clinic Address: " + listOfClin[x].Address + "\n" +
                    "Clinic Website: " + listOfClin[x].Website + "\n";
                message2 += "----------------------------------------------------------------------------------------------------------------- \n";

            }
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
                
                    var body = message;

                    var smtpMess2 = SmtpModel.SmtpMail(senderInfo);
                    mess.From = senderEmail;
                    mess.To.Add(receiverEmail);
                    mess.Subject = "New Patient";
                    mess.Body = message2;
                    if (mess.To != null)
                    {
                        using (smtpMess2)
                        {
                            smtpMess2.Send(mess);
                        }
                    }

                    var smtpMess1 = SmtpModel.SmtpMail(senderInfo);
                    var mess2 = new MailMessage();
                    mess2.From = senderEmail;
                    for (int x = 0; x < clinicName.Length; x++)
                    {
                        mess2.To.Add(receiverEmail = new MailAddress(listOfClin[x].Email, listOfClin[x].Name));

                    }
                    mess2.Subject = "Clinic Info";
                    mess2.Body = message;

                    if(mess2.To != null)
                    {
                         using (smtpMess1)
                        {
                            smtpMess1.Send(mess2);
                        }
                    }
                 

                   // Patient.AddPatient(patient);
   
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
            tModel.GuidelineValue = Criteria.GetCriteria("+IncomePerPerson"); 
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