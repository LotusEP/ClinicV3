using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicV2.Models
{
    public class Patient
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string CellPhone { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Email { get; set; }
        public int Income { get; set; }
        public int Household { get; set; }
        
        
        public static void AddPatient(Patient patient)
            {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);

            string sql;
            //sql= "IF EXISTS(SELECT * FROM Req WHERE ReqName="+req.Name+") Update Req"
            //sql = "Insert Into Req Values(@Aug1,@Aug2,@State,@ReqName)";
            sql = "Insert Into PatientDB (FirstName,LastName,Street,CIty,Zip,State,Email,PhoneNumber) Values(@FirstName,@LastName" +
                "@Street,@City,@State,@Email,@Cell";
           


            MySqlCommand cmm = new MySqlCommand(sql, cnn);

            cmm.Parameters.AddWithValue("@FirstName", patient.FirstName.ToString());
            cmm.Parameters.AddWithValue("@LastName", patient.LastName.ToString());
            cmm.Parameters.AddWithValue("@Street",patient.Street.ToString());
            cmm.Parameters.AddWithValue("@City",patient.City.ToString());
            cmm.Parameters.AddWithValue("@Zip",patient.Zip.ToString());
            cmm.Parameters.AddWithValue("@State",patient.State.ToString());
            cmm.Parameters.AddWithValue("@Email",patient.Email.ToString());
            cmm.Parameters.AddWithValue("@Cell",patient.CellPhone.ToString());
            
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();



            


            }
    }

   
}

