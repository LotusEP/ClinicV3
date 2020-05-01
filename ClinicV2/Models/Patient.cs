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
        public int PatientID { get; set; }
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
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        public string Reference { get; set; }
        public string Income { get; set; }
        public string Household { get; set; }
        public string Insurance { get; set; }


        public Patient() { }
        public Patient(string fName, string lName, string street, string cellphone, string city, string zip, string state, string email, string phone) {
            FirstName = fName;
            LastName = lName;
            Street = street;
            CellPhone = phone;
            City = city;
            Zip = zip;
            State = state;
            Email = email;
            
        }

        //add the patient to the database
        public static void AddPatient(Patient patient)
            {
            MySqlConnection cnn = DataModel.getSqlConnection();

            string sql;
            string sql1, sql2;

            sql1 = "Insert Into PatientDB (FirstName,LastName,Street,City,Zip,State,Email,PhoneNumber,Ref";
            sql2 = ") Values(@FirstName,@LastName,@Street,@City,@Zip,@State,@Email,@Cell,@Ref";
            if (patient.Income != null)
            {
                sql1 += ",Income";
                sql2 += ",@Income";
            }
            if (patient.Household != null)
            {
                sql1 += ",Household";
                sql2 += ",@Household";
            }

            if (patient.Insurance != null)
            {
                sql1 += ",Insurance";
                sql2 += ",@Insurance";
            }

            sql = sql1 + sql2 + ")";

            MySqlCommand cmm = new MySqlCommand(sql, cnn);

            cmm.Parameters.AddWithValue("@FirstName", patient.FirstName.ToString());
            cmm.Parameters.AddWithValue("@LastName", patient.LastName.ToString());
            cmm.Parameters.AddWithValue("@Street",patient.Street.ToString());
            cmm.Parameters.AddWithValue("@City",patient.City.ToString());
            cmm.Parameters.AddWithValue("@Zip",patient.Zip.ToString());
            cmm.Parameters.AddWithValue("@State",patient.State.ToString());
            cmm.Parameters.AddWithValue("@Email",patient.Email.ToString());
            cmm.Parameters.AddWithValue("@Cell",patient.CellPhone.ToString());
            cmm.Parameters.AddWithValue("@Ref", patient.Reference.ToString());
            if (patient.Income != null)
            {
  
                cmm.Parameters.AddWithValue("@Income", patient.Income.ToString());
            }
            if (patient.Household != null)
            {

                cmm.Parameters.AddWithValue("@Household",patient.Household.ToString());
            }
            if (patient.Insurance != null)
            {

                cmm.Parameters.AddWithValue("@Insurance", patient.Insurance.ToString());
            }
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();



            


            }


        public static List<Patient> ListofPatient()
        {
            List<Patient> listofPatient = new List<Patient>();
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Select * From PatientDB";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cnn.Open();
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Patient toAdd = new Patient();
             


                toAdd.PatientID = Int32.Parse(rdr.GetValue(0).ToString());
                toAdd.FirstName = rdr.GetValue(1).ToString();
                toAdd.LastName = rdr.GetValue(2).ToString();
                toAdd.Street = rdr.GetValue(3).ToString();
                toAdd.City = rdr.GetValue(4).ToString();
                toAdd.Zip = rdr.GetValue(5).ToString();
                toAdd.State = rdr.GetValue(6).ToString();
                toAdd.Email = rdr.GetValue(7).ToString();
                toAdd.CellPhone = rdr.GetValue(8).ToString();
                toAdd.Income = rdr.GetValue(9).ToString();           
                toAdd.Household = rdr.GetValue(10).ToString();
                toAdd.Insurance = rdr.GetValue(11).ToString();
                toAdd.Reference = rdr.GetValue(12).ToString();
                listofPatient.Add(toAdd);

            }


            cnn.Close();

            return listofPatient;
        }

        public static Patient GetPatientInfo(int ID)
        {
            Patient PatientInfo = new Patient();
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Select * From PatientDB Where PatientID = @ID ";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);

            cmd.Parameters.AddWithValue("@ID", ID);
            cnn.Open();
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {


                PatientInfo.PatientID = Int32.Parse(rdr.GetValue(0).ToString());
                PatientInfo.FirstName = rdr.GetValue(1).ToString();
                PatientInfo.LastName = rdr.GetValue(2).ToString();
                PatientInfo.Street = rdr.GetValue(3).ToString();
                PatientInfo.City = rdr.GetValue(4).ToString();
                PatientInfo.Zip = rdr.GetValue(5).ToString();
                PatientInfo.State = rdr.GetValue(6).ToString();
                PatientInfo.Email = rdr.GetValue(7).ToString();
                PatientInfo.CellPhone = rdr.GetValue(8).ToString();
                PatientInfo.Income = rdr.GetValue(9).ToString();
                PatientInfo.Household = rdr.GetValue(10).ToString();
                PatientInfo.Insurance = rdr.GetValue(11).ToString();
                PatientInfo.Reference = rdr.GetValue(12).ToString();


            }


            cnn.Close();

            return PatientInfo;
        }
    }

   
}

