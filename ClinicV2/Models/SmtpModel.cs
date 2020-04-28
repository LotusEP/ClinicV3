using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicV2.Models
{
    public class SmtpModel : clinicModel
    {
        public int smtpID { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public string Smtp { get; set; }
        [Required]
        public int Port { get; set; }
        public SmtpModel(string smtp, int port ,int ID, string provider)
        {
            Provider = provider;
            smtpID = ID;
            Smtp = smtp;
            Port = port;
        }
        public SmtpModel(string smtp, int port)
        {
            Smtp = smtp;
            Port = port;
        }
        public SmtpModel()
        {
        }
        //get the all stmp info into one
        public static SmtpModel getStmpInfo(int clinicID)
        {
            SmtpModel SmtpInfo = new SmtpModel();

            clinicModel tempEmail = GetEmail(clinicID);
            SmtpModel tempStmp = GetStmp(tempEmail.Email);

            SmtpInfo.Smtp = tempStmp.Smtp;
            SmtpInfo.Port = tempStmp.Port;
            SmtpInfo.Email = tempEmail.Email;
            SmtpInfo.MailPassword = tempEmail.MailPassword;

            return SmtpInfo;
        }
        //get the email and pass
        public static SmtpModel GetStmp(string mail)
        {
            SmtpModel smtpOb = new SmtpModel();
            string sql;
            MySqlConnection cnn = DataModel.getSqlConnection();
            int start = mail.IndexOf("@", 0);

            int end = mail.LastIndexOf(".");

            int val = end - start;

            string provider = mail.Substring((start + 1), (val-1));
            char.ToUpper(provider[0]);

            sql = "Select * from EmailSmtp Where Provider = @Provider;";

            MySqlCommand cmm = new MySqlCommand(sql, cnn);

            cmm.Parameters.AddWithValue("@Provider", provider);

            MySqlDataReader rdr = null;
            cnn.Open();
            rdr = cmm.ExecuteReader();

            while (rdr.Read())
            {
                smtpOb.Smtp = rdr.GetValue(2).ToString();
                smtpOb.Port = Int32.Parse(rdr.GetValue(3).ToString());

            }
           
            cnn.Close();

            return smtpOb;
        }
        //get stmp host and port number

        public static clinicModel GetEmail(int clinicID)
        {
            clinicModel clinic = new clinicModel();
            string sql;
            MySqlConnection cnn = DataModel.getSqlConnection();


            sql = "Select Email, mailPassword, Name from Clinic Where ClinicID = @ID";

           MySqlCommand cmm = new MySqlCommand(sql, cnn);

            cmm.Parameters.AddWithValue("@ID", clinicID);

            MySqlDataReader rdr = null;
            cnn.Open();
            rdr = cmm.ExecuteReader();

            while (rdr.Read())
            {

                clinic.Email = rdr.GetValue(0).ToString();
                clinic.MailPassword = rdr.GetValue(1).ToString();
                clinic.Name = rdr.GetValue(2).ToString();

            }
           
            cnn.Close();

            return clinic;

        }
        //add new Stmp
        public static void AddStmp(SmtpModel stmpNew)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Insert into EmailSmtp (Provider,SMTP,Port) Value(@Provider,@Smtp,@Port)";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Provider", stmpNew.Provider);
            cmd.Parameters.AddWithValue("@Smtp", stmpNew.Smtp);
            cmd.Parameters.AddWithValue("@Port", stmpNew.Port);

            cmd.ExecuteNonQuery();
            cnn.Close();



        }

        //getting the stmp
        public static SmtpModel GetStmp(int ID)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Select * From EmailSmtp Where SmtpID =@ID";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@ID", ID);
            MySqlDataReader rdr = null;

            rdr = cmd.ExecuteReader();

            SmtpModel smtpModel = new SmtpModel(); ;
            while (rdr.Read())
            {
                smtpModel.smtpID = Int32.Parse(rdr.GetValue(0).ToString());
                smtpModel.Provider = rdr.GetValue(1).ToString();
                smtpModel.Smtp = rdr.GetValue(2).ToString();
                smtpModel.Port = Int32.Parse(rdr.GetValue(3).ToString());
            }
            cnn.Close();

            return smtpModel;

        }
        public static List<SmtpModel> getSmtpList()
        {
            List<SmtpModel> newList = new List<SmtpModel>();
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Select * From EmailSmtp";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cnn.Open();
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();
           
            while (rdr.Read())
            {
                newList.Add(new SmtpModel {
                    smtpID =   Int32.Parse(rdr.GetValue(0).ToString()),
                    Provider = rdr.GetValue(1).ToString(),
                    Smtp = rdr.GetValue(2).ToString(),
                    Port =   Int32.Parse(rdr.GetValue(3).ToString())

                });
            }

           
            cnn.Close();

            return newList;
        }
        //update stmp
        public static void EditStmp(SmtpModel stmpNew)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Update EmailSmtp Set Provider =@Provider, SMTP = @Smtp, Port = @Port Where SmtpID =@ID";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Provider", stmpNew.Provider);
            cmd.Parameters.AddWithValue("@Smtp", stmpNew.Smtp);
            cmd.Parameters.AddWithValue("@Port", stmpNew.Port);
            cmd.Parameters.AddWithValue("@ID", stmpNew.smtpID);

            cmd.ExecuteNonQuery();
            cnn.Close();

        }


        public static void DeleteStmp(int ID) {
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Delete from EmailSmtp Where SmtpID = @ID";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            cnn.Close();

        }
    }

}