
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicV2.Models
{
    public class clinicModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
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
                    Name = rdr.GetValue(0).ToString(),
                   
                    Email = rdr.GetValue(1).ToString(),
                    PhoneNumber = rdr.GetValue(2).ToString(),
                    Address = rdr.GetValue(3).ToString() + rdr.GetValue(4).ToString() + rdr.GetValue(5).ToString() + rdr.GetValue(6).ToString(),
                    AddrName = rdr.GetValue(7).ToString(),
                    Req = new List<Criteria>()



                });
            }


            foreach (clinicModel CMD in listofClinic)
            {
                CMD.Req = Criteria.GetReqList(CMD.Name);
            }


            cnn.Close();

            //string connString;
            //SqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //cnn = new SqlConnection(connString);
            //cnn.Open();
            //SqlCommand comm;
            //SqlDataReader dataReader;
            //string sql;

            //sql = "Select * from clinicDB";
            //comm = new SqlCommand(sql, cnn);

            //dataReader = comm.ExecuteReader();
            //if (dataReader != null)
            //{
            //    while (dataReader.Read())
            //    {
            //        listofClinic.Add(new clinicModel
            //        {
            //            Name = dataReader.GetValue(1).ToString(),
            //            ID = dataReader.GetValue(0).ToString(),
            //            Email = dataReader.GetValue(6).ToString(),
            //            PhoneNumber = dataReader.GetValue(7).ToString(),
            //            Address = dataReader.GetValue(2).ToString() + dataReader.GetValue(3).ToString() + dataReader.GetValue(4).ToString() + dataReader.GetValue(5).ToString(),
            //            AddrName = dataReader.GetValue(8).ToString(),
            //            Req = new List<Criteria>()

            //        }); 

            //    }

            //}


            return listofClinic;
        }

        public static void CreateClinic(clinicModel newClinic)
        {

            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);

            MySqlCommand comm;

            string sql;
            //sql= "IF EXISTS(SELECT * FROM Req WHERE ReqName="+req.Name+") Update Req"
            //sql = "Insert Into Req Values(@Aug1,@Aug2,@State,@ReqName)";
            sql = "Insert Into Clinic (Name,Email,PhoneNumber,Street,City,State,Zip,NameAbbrev) Values('"+
                newClinic.Name.ToString() +
                "','" + newClinic.Email.ToString() +
                "','" + newClinic.PhoneNumber.ToString() +
                "','" + newClinic.Street.ToString() +
                "','" + newClinic.City.ToString() +
                "','" + newClinic.state.ToString() +
                "','" + Int32.Parse(newClinic.zip) +      
                "','" + newClinic.AddrName.ToString() + "')";
            comm = new MySqlCommand(sql, cnn);

            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

        }

        public static void deleteClinic(string name)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql;
            sql = "Delete From Clinic Where Name ='" + name + "';";
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

        }


    }


}
