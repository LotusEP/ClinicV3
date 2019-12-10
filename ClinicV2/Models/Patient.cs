using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicV2.Models
{
    public class Patient
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Street { get; set; }
        public string CellPhone { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string Email { get; set; }

        public int Income { get; set; }
        public int Household { get; set; }
        
        
        public static void AddPatient(Patient patient)
            {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);

            MySqlCommand comm;

            string sql;
            //sql= "IF EXISTS(SELECT * FROM Req WHERE ReqName="+req.Name+") Update Req"
            //sql = "Insert Into Req Values(@Aug1,@Aug2,@State,@ReqName)";
            sql = "Insert Into PatientDB (FirstName,LastName,Street,CIty,Zip,State,Email,PhoneNumber) Values('" + 
                patient.FName.ToString() + 
                "','" + patient.LName.ToString() +
                "','" + patient.Street.ToString() +
                "','" + patient.City.ToString() +
                "','" + patient.Zip.ToString() +
                "','" + patient.State.ToString() +
                "','" + patient.Email.ToString() +
                "','" + patient.CellPhone.ToString() + "')";
            comm = new MySqlCommand(sql, cnn);

            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();



            


            }
    }

   
}

