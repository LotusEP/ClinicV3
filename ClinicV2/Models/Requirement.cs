
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ClinicV2.Models
{
    public class Requirement
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Aug1 { get; set; }
        public string Aug2 { get; set; }
        public string State { get; set; }



        public static List<Requirement> GetReqList()
        {
            List<Requirement> listofReq = new List<Requirement>();

            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            
            
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();

            string sql = "Select * from Req ORDER BY ReqID";

            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                listofReq.Add(new Requirement
                {
                    Name = rdr.GetValue(4).ToString(),
                    ID = Int32.Parse(rdr.GetValue(0).ToString()),
                    Aug1 = rdr.GetValue(1).ToString(),
                    Aug2 = rdr.GetValue(2).ToString(),
                    State = rdr.GetValue(3).ToString()



                });
            }

      


            cnn.Close();


            return listofReq;
        }

        public void CreateReq(Requirement req)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);
    
            MySqlCommand comm;
       
            string sql;
            //sql= "IF EXISTS(SELECT * FROM Req WHERE ReqName="+req.Name+") Update Req"
            //sql = "Insert Into Req Values(@Aug1,@Aug2,@State,@ReqName)";
            sql = "Insert Into Req (Aug1,Aug2,State,ReqName) Values('"+req.Aug1.ToString()+"','"+ req.Aug2.ToString()+"','"+req.State.ToString()+"','"+ req.Name.ToString()+"')";
            comm = new MySqlCommand(sql, cnn);

            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

            //using (SqlCommand cmm = new SqlCommand(sql, cnn))
            //{
             
            //    cmm.Parameters.AddWithValue("@Aug1", req.Aug1);
            //    cmm.Parameters.AddWithValue("@Aug2", req.Aug2);
            //    cmm.Parameters.AddWithValue("@State", req.State);
            //    cmm.Parameters.AddWithValue("@ReqName", req.Name);
            //    cnn.Open();
            //    cmm.ExecuteNonQuery();
            //    cnn.Close();

            //}
  
        }
    }
}
