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
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        public int Income { get; set; }
        public int Household { get; set; }
        
        //add the patient to the database
        public static void AddPatient(Patient patient)
            {
            MySqlConnection cnn = DataModel.getSqlConnection();

            string sql;

            sql = "Insert Into PatientDB (FirstName,LastName,Street,City,Zip,State,Email,PhoneNumber) Values(@FirstName,@LastName," +
                "@Street,@City,@Zip,@State,@Email,@Cell)";
           


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

