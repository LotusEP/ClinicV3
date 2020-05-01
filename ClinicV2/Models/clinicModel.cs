
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicV2.Models
{
    public class clinicModel
    {
        public int ClinicID { get; set; }
        [Required]
        public string Name { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string zip { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Website { get; set; }
        
        public string MailPassword { get; set; }

        public string AddrName { get; set; }

        public List<Criteria> Req { get; set; }



        public static List<clinicModel> GetClinicList(string arug)
        {
            List<clinicModel> listofClinic = new List<clinicModel>();

            MySqlConnection cnn = DataModel.getSqlConnection();

            MySqlDataReader rdr = null;

            cnn.Open();
            string sql = "Select * from Clinic";

            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                clinicModel toAdd = new clinicModel
                {
                    ClinicID = Int32.Parse(rdr.GetValue(0).ToString()),
                    Name = rdr.GetValue(1).ToString(),
                    Email = rdr.GetValue(2).ToString(),
                    PhoneNumber = rdr.GetValue(3).ToString(),
                    Address = rdr.GetValue(4).ToString() + " " + rdr.GetValue(5).ToString() + " " + rdr.GetValue(6).ToString() + " " + rdr.GetValue(7).ToString(),
                    AddrName = rdr.GetValue(8).ToString(),
                    Website = rdr.GetValue(9).ToString(),
                    Req = new List<Criteria>()
                };

                if (arug == "Password")
                {
                   toAdd.MailPassword = rdr.GetValue(10).ToString();
                }

                listofClinic.Add(toAdd);
            }


            foreach (clinicModel CMD in listofClinic)
            {
                CMD.Req = Criteria.GetReqList(CMD.ClinicID.ToString());
            }


            cnn.Close();



            return listofClinic;
        }

        public static void CreateClinic(clinicModel newClinic)
        {

            MySqlConnection cnn = DataModel.getSqlConnection();


            string sql ="Empty";
            string sqlp1 = "Insert Into Clinic (Name,PhoneNumber,Street,City,State,Zip,Website";
            string sqlp2 =") Values(@Name, @PhoneNumber, @Street, @City, @State, @Zip, @Website";
            if (newClinic.AddrName == null)
            {
                sqlp1 += ",NameAbbrev";
                sqlp2 += ",@AddrrName";

            }
            if (newClinic.Email == null)
            {
                sqlp1 += ",Email";
                sqlp2 += ",@Email";
            }
            if (newClinic.MailPassword == null)
            {
                sqlp1 += ",mailPassword";
                sqlp2 += ",@MailP";
            }
            sql = sqlp1 + sqlp2 + ")";

            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@Name", newClinic.Name.ToString());
            cmm.Parameters.AddWithValue("@PhoneNumber", newClinic.PhoneNumber.ToString());
            cmm.Parameters.AddWithValue("@Street", newClinic.Street.ToString());
            cmm.Parameters.AddWithValue("@City", newClinic.City.ToString());
            cmm.Parameters.AddWithValue("@State", newClinic.state.ToString());
            cmm.Parameters.AddWithValue("@Zip", Int32.Parse(newClinic.zip));
            cmm.Parameters.AddWithValue("@Website", newClinic.Website.ToString());
            if (newClinic.AddrName == null)
            {
                newClinic.AddrName = " ";
                  cmm.Parameters.AddWithValue("@AddrrName", newClinic.AddrName.ToString());
            }
            if (newClinic.Email == null)
            {
                newClinic.Email = " ";
                cmm.Parameters.AddWithValue("@Email", newClinic.Email.ToString());
            }
            if (newClinic.MailPassword == null)
            {
                newClinic.MailPassword= " ";
                cmm.Parameters.AddWithValue("@MailP", newClinic.MailPassword.ToString());
            }
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

        }

        public static void DeleteClinic(int ID)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();


            string sql;
            sql = "Delete From Clinic Where ClinicID =@ID;";
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@ID", ID);
           
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

        }

        public static clinicModel GetClinic(int Id)
        {
            clinicModel clinic = new clinicModel();
            MySqlConnection cnn = DataModel.getSqlConnection();

            MySqlDataReader rdr = null;

            cnn.Open();
            string sql = "Select * from Clinic Where ClinicID = @ID";

            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@ID", Id);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                
                clinic.ClinicID = Int32.Parse(rdr.GetValue(0).ToString());
                clinic.Name = rdr.GetValue(1).ToString();
                clinic.Email = rdr.GetValue(2).ToString();
                clinic.PhoneNumber = rdr.GetValue(3).ToString();
                clinic.Street = rdr.GetValue(4).ToString();
                clinic.City = rdr.GetValue(5).ToString();
                clinic.state = rdr.GetValue(6).ToString();
                clinic.zip = rdr.GetValue(7).ToString();
                clinic.AddrName = rdr.GetValue(8).ToString();
                clinic.Website = rdr.GetValue(9).ToString();
                clinic.MailPassword = rdr.GetValue(10).ToString();

            }

            cnn.Close();
            return clinic;
        }
        public static void EditClinic(clinicModel Clinic)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();


            cnn.Open();
            string sql = "UPDATE Clinic Set Name=@Name, PhoneNumber=@PhoneNumber, " +
                 "Street=@Street, City=@City, State=@State, Zip=@Zip , Website=@Website";

            if (Clinic.Email == null)
            {
                Clinic.Email = "None";
                sql += " ,Email=@Email";
            }
            else { sql += " ,Email=@Email"; }

            if (Clinic.AddrName == null)
            {
                Clinic.AddrName = "None";
                sql += "  ,NameAbbrev=@AddrrName";

            }
            else { sql += "  ,NameAbbrev=@AddrrName"; }
            if (Clinic.MailPassword == null)
            {
                Clinic.MailPassword = "None";
                sql += "  ,mailPassword=@MailP";
            }
            else {
                sql += "  ,mailPassword=@MailP";
            }
            sql += " Where ClinicID= @ID";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);

            
            cmd.Parameters.AddWithValue("@ID", Clinic.ClinicID);
            cmd.Parameters.AddWithValue("@Name", Clinic.Name.ToString());
            cmd.Parameters.AddWithValue("@PhoneNumber", Clinic.PhoneNumber.ToString());
            cmd.Parameters.AddWithValue("@Street", Clinic.Street.ToString());
            cmd.Parameters.AddWithValue("@City", Clinic.City.ToString());
            cmd.Parameters.AddWithValue("@State", Clinic.state.ToString());
            cmd.Parameters.AddWithValue("@Zip", Int32.Parse(Clinic.zip));
            cmd.Parameters.AddWithValue("@Website", Clinic.Website.ToString());

            if (Clinic.Email != null)
            {    
                cmd.Parameters.AddWithValue("@Email", Clinic.Email.ToString());
            }

            if (Clinic.AddrName != null)
            {
                cmd.Parameters.AddWithValue("@AddrrName", Clinic.AddrName.ToString());
            }

            if (Clinic.MailPassword != null)
            {
                cmd.Parameters.AddWithValue("@MailP", Clinic.MailPassword.ToString());
            }


            cmd.ExecuteNonQuery();
            cnn.Close();



        }


    }


}
