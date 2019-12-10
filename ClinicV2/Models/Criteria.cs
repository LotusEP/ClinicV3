using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicV2.Models
{
    public class Criteria
    {
        public int CriteriaID { get; set; }
        public string clinicName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Criteria()
        { }

        public Criteria(int ID, string C_Name, string Req_Name, string Req_Value)
        {
            CriteriaID = ID;
            clinicName = C_Name;
            Name = Req_Name;
            Value = Req_Value;
        }

        public static List<Criteria> GetReqList(string clinic)
        {
            List<Criteria> listofReq = new List<Criteria>();

            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();
            if (clinic != "-10")
            {
                string sql = "Select Clinic.Name, Criteria.CriteriaName, CriteriaOption.Value From ClinicCriteria " +
                              "Join Clinic On Clinic.Name = ClinicCriteria.Clinic_Name " +
                              "Join Criteria on Criteria.CriteriaName = ClinicCriteria.Criteria_Name " +
                              "Join CriteriaOption on CriteriaOption.CriteriaOp_Name = ClinicCriteria.Criteria_Name "+ 
                              "Where Clinic_Name =  '" + clinic + "'" ;

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listofReq.Add(new Criteria
                    {
                        Name = rdr.GetValue(0).ToString(),
                        Value = rdr.GetValue(2).ToString()

                    });

                }


            }
            else if (clinic == "old")
            {
                string sql = "Select * from Criteria";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    listofReq.Add(new Criteria
                    {                    
                        Name = rdr.GetValue(0).ToString(),

                    });
                }


            }

            else
            {
                string sql = "Select Clinic.Name, Criteria.CriteriaName,ClinicCriteriaID, CriteriaOption.Value From ClinicCriteria  " +
                            "Join Clinic On Clinic.Name = ClinicCriteria.Clinic_Name "+
                            "Join Criteria on Criteria.CriteriaName = ClinicCriteria.Criteria_Name "+
                            "Join CriteriaOption on CriteriaOption.CriteriaOp_Name = ClinicCriteria.Criteria_Name "; 

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    listofReq.Add(new Criteria
                    {
                        clinicName = rdr.GetValue(0).ToString(),
                        CriteriaID = Convert.ToInt32(rdr.GetValue(2)),
                        Name = rdr.GetValue(1).ToString(),
                        Value = rdr.GetValue(3).ToString()


                    }) ;
                }


            }
       



            cnn.Close();


            return listofReq;
        }

        public static String AddCriteria(Criteria newCriteria)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);
            String Mess = null;
         

            string sql;
            //sql= "IF EXISTS(SELECT * FROM Req WHERE ReqName="+req.Name+") Update Req"
            //sql = "Insert Into Req Values(@Aug1,@Aug2,@State,@ReqName)";
            sql = "Select count(1) " +
                    "From ClinicCriteria " +
                    "Join Clinic On Clinic.Name = ClinicCriteria.Clinic_Name " +
                    "Join Criteria on Criteria.CriteriaName = ClinicCriteria.Criteria_Name " +
                    "Join CriteriaOption on CriteriaOption.CriteriaOp_Name = ClinicCriteria.Criteria_Name " +
                    "Where Clinic_Name = '" + newCriteria.clinicName + "' and CriteriaName = '" + newCriteria.Name + "' and CriteriaOption.Value = '" + newCriteria.Value + "'";


            MySqlDataReader rdr = null;



            int val = 0;
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            rdr = cmm.ExecuteReader();
            while (rdr.Read())
            {
                val = Convert.ToInt32(rdr.GetValue(0).ToString());
            }
            cnn.Close();
            if (val > 0)
            {
                Mess = "Criteria Already exist!";
            }
            else if (val == 0)
            {
                sql = "Select count(1 ) From Criteria Where CriteriaName = '" + newCriteria.Name + "'";
                cmm = new MySqlCommand(sql, cnn);
                cnn.Open();
                rdr = cmm.ExecuteReader();
                while (rdr.Read())
                {
                    val = Convert.ToInt32(rdr.GetValue(0).ToString());
                }
                cnn.Close();
                if (val > 0)
                {
                    sql = "Select count(1 ) From CriteriaOption Where Value = '" + newCriteria.Value + "'";
                    cmm = new MySqlCommand(sql, cnn);
                    cnn.Open();
                    rdr = cmm.ExecuteReader();
                    while (rdr.Read())
                    {
                        val = Convert.ToInt32(rdr.GetValue(0).ToString());
                    }
                    cnn.Close();
                  
                    if (val > 0)
                    {
                        sql = "Begin; " +

                       "Insert Into ClinicCriteria(Clinic_Name, Criteria_Name) Values('" + newCriteria.clinicName + "','" + newCriteria.Name + "'); " +
                       "Commit; ";
                        cmm = new MySqlCommand(sql, cnn);
                        cnn.Open();
                        cmm.ExecuteNonQuery();
                    }
                    else
                    {
                        sql = "Begin; " +

                      "Insert Into CriteriaOption (CriteriaOp_Name,Value) Values('" + newCriteria.Name + "','" + newCriteria.Value + "'); " +
                      "Insert Into ClinicCriteria(Clinic_Name, Criteria_Name) Values('" + newCriteria.clinicName + "','" + newCriteria.Name + "'); " +
                      "Commit ;";
                        cmm = new MySqlCommand(sql, cnn);
                        cnn.Open();
                        cmm.ExecuteNonQuery();
                    }
                    cnn.Close();

                }

            }
            else
            {
                sql = "Begin; " +
                      "Insert Into Criteria (CriteriaName) Values('" + newCriteria.Name + "')" +
                      "Insert Into CriteriaOption (CriteriaOp_Name,Value) Values('" + newCriteria.Name + "','" + newCriteria.Value + "')" +
                      "Insert Into ClinicCriteria(Clinic_Name, Criteria_Name) Values('" + newCriteria.clinicName + "','" + newCriteria.Name + "'); " +
                      "Commit ;";
                cmm = new MySqlCommand(sql, cnn);
                cnn.Open();
                cmm.ExecuteNonQuery();
                cnn.Close();
            }



   
          
    

            return Mess;
        }

        public static void DeleteCriteria(Criteria oldCriteria)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql;
            sql = "Delete From ClinicCriteria Where ClinicCriteriaID =" + oldCriteria.CriteriaID + ";";
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm = new MySqlCommand(sql, cnn);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();


        }

        public static Criteria GetCriteria(int ID)
        {
            Criteria clinicReq = null;

            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();
            if (ID != -10)
            {
                string sql = "Select Clinic.Name, Criteria.CriteriaName, CriteriaOption.Value,ClinicCriteriaID  From ClinicCriteria " +
                              "Join Clinic On Clinic.Name = ClinicCriteria.Clinic_Name " +
                              "Join Criteria on Criteria.CriteriaName = ClinicCriteria.Criteria_Name " +
                              "Join CriteriaOption on CriteriaOption.CriteriaOp_Name = ClinicCriteria.Criteria_Name " +
                              "Where ClinicCriteriaID =  '" + ID + "'";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int tempID = Convert.ToInt32(rdr.GetValue(3));
                    string clin_name = rdr.GetValue(0).ToString();
                    string crit_Name = rdr.GetValue(1).ToString();
                    string crit_value = rdr.GetValue(2).ToString();
                    clinicReq = new Criteria(tempID, clin_name, crit_Name, crit_value);


         
                }


            }
            cnn.Close();
            return clinicReq;
        }

    }
}