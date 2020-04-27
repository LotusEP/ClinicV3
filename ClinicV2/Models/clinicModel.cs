
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
        [Required]
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

        //public int Age { get; set; }

        //public int Income { get; set; }
        //public Boolean Insurance { get; set; }

        //public string Housing { get; set; }

   

        public string AddrName { get; set; }

        public List<Criteria> Req { get; set; }



        public static List<clinicModel> GetClinicList()
        {
            List<clinicModel> listofClinic = new List<clinicModel>();

            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();
            string sql = "Select * from Clinic";

            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                listofClinic.Add(new clinicModel
                {
                    ClinicID = Int32.Parse(rdr.GetValue(0).ToString()),
                    Name = rdr.GetValue(1).ToString(),
                   
                    Email = rdr.GetValue(2).ToString(),
                    PhoneNumber = rdr.GetValue(3).ToString(),
                    Address = rdr.GetValue(4).ToString() + rdr.GetValue(5).ToString() + rdr.GetValue(6).ToString() + rdr.GetValue(7).ToString(),
                    AddrName = rdr.GetValue(8).ToString(),
                    Req = new List<Criteria>()



                });
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

            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);

     

            string sql ="Empty";
           
            if (newClinic.AddrName != null)
            {
                sql = "Insert Into Clinic (Name,Email,PhoneNumber,Street,City,State,Zip,NameAbbrev) Values(@Name,@Email,@PhoneNumber,@Street,@City,@State,@Zip,@AddrrName)";
            
            }
            else
            {
                sql = "Insert Into Clinic (Name,Email,PhoneNumber,Street,City,State,Zip) Values(@Name,@Email,@PhoneNumber,@Street,@City,@State,@Zip)";

            }
           


            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@Name", newClinic.Name.ToString());
            cmm.Parameters.AddWithValue("@Email", newClinic.Email.ToString());
            cmm.Parameters.AddWithValue("@PhoneNumber", newClinic.PhoneNumber.ToString());
            cmm.Parameters.AddWithValue("@Street", newClinic.Street.ToString());
            cmm.Parameters.AddWithValue("@City", newClinic.City.ToString());
            cmm.Parameters.AddWithValue("@State", newClinic.state.ToString());
            cmm.Parameters.AddWithValue("@Zip", Int32.Parse(newClinic.zip));

            if (newClinic.AddrName != null)
            {
                cmm.Parameters.AddWithValue("@AddrrName", newClinic.AddrName.ToString());
            }
                cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

        }

        public static void DeleteClinic(string name)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql;
            sql = "Delete From Clinic Where Name =@Name;";
            MySqlCommand cmm = new MySqlCommand(sql, cnn);

            cmm.Parameters.AddWithValue("@Name", name);
            cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

        }

        public static clinicModel GetClinic(int Id)
        {
            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            clinicModel clinic = new clinicModel();
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

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
 
            }


            return clinic;
        }
        public static void EditClinic(clinicModel Clinic)
        {
            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

  
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();
            string sql = "UPDATE Clinic Set Name=@Name, Email=@Email, PhoneNumber=@PhoneNumber, " + 
                 "Street=@Street, City=@City, State=@State, Zip=@Zip ,NameAbbrev=@AddrrName Where ClinicID= @ID";

            MySqlCommand cmd = new MySqlCommand(sql, cnn);
           
 
            cmd.Parameters.AddWithValue("@ID", Clinic.ClinicID);
            cmd.Parameters.AddWithValue("@Name", Clinic.Name.ToString());
            cmd.Parameters.AddWithValue("@Email", Clinic.Email.ToString());
            cmd.Parameters.AddWithValue("@PhoneNumber", Clinic.PhoneNumber.ToString());
            cmd.Parameters.AddWithValue("@Street", Clinic.Street.ToString());
            cmd.Parameters.AddWithValue("@City", Clinic.City.ToString());
            cmd.Parameters.AddWithValue("@State", Clinic.state.ToString());
            cmd.Parameters.AddWithValue("@Zip", Int32.Parse(Clinic.zip));
            cmd.Parameters.AddWithValue("@AddrrName", Clinic.AddrName.ToString());

            cmd.ExecuteNonQuery();



        }


    }


}
