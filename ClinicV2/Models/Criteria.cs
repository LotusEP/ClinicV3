using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicV2.Models
{
    public class Criteria
    {
        public int CriteriaID { get; set; }
        public string clinicName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }

        public Criteria()
        { }
        public Criteria(string Req_Name, string Req_Value)
        {
            Name = Req_Name;
            Value = Req_Value;
        }
        public Criteria(int ID, string Req_Name, string Req_Value)
        {
            CriteriaID = ID;
            Name = Req_Name;
            Value = Req_Value;
        }

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
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);
            MySqlDataReader rdr = null;
            cnn.Open();
            if (clinic == "old")
            {
                string sql = "Select CriteriaName from Criteria";
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
            else if (clinic != "-10")
            {
                int val = Int32.Parse(clinic);
                string sql = "Select ClinicCriteriaID,  Clinic.Name, Criteria.CriteriaName, CriteriaOption.Value From ClinicCriteria" +
                             " Join CriteriaOption on CriteriaOption.CriteriaOptionID = ClinicCriteria.Criteria_OptionID" +
                             " join Clinic on Clinic.ClinicID = ClinicCriteria.Clinic_ID" +
                             " join Criteria on ClinicCriteria.Criteria_ID = Criteria.CriteriaID"+
                            " Where Clinic.ClinicID =@Val;"; 
    

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@Val", val);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listofReq.Add(new Criteria
                    {
                        Name = rdr.GetValue(2).ToString(),
                        Value = rdr.GetValue(3).ToString()

                    });

                }
            }
            else
            {
                string sql = "Select ClinicCriteriaID,  Clinic.Name, Criteria.CriteriaName, CriteriaOption.Value From ClinicCriteria" +
                             " Join CriteriaOption on CriteriaOption.CriteriaOptionID = ClinicCriteria.Criteria_OptionID" +
                             " join Clinic on Clinic.ClinicID = ClinicCriteria.Clinic_ID" +
                             " join Criteria on ClinicCriteria.Criteria_ID = Criteria.CriteriaID";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listofReq.Add(new Criteria
                    {
                        CriteriaID = Int32.Parse(rdr.GetValue(0).ToString()),
                        clinicName = rdr.GetValue(1).ToString(),
                        Name = rdr.GetValue(2).ToString(),
                        Value = rdr.GetValue(3).ToString(),


                    });
                }


            }

            cnn.Close();
            return listofReq;
        }

        public static String AddCriteria(Criteria newCriteria, string mess)
        {
            String Mess = null;
            if (newCriteria.Name != null && newCriteria.Name != "" && newCriteria.Value != null && newCriteria.Value != "")
            {
                string connString;
                MySqlConnection cnn;
                connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
                cnn = new MySqlConnection(connString);
            
                MySqlCommand cmm;
                MySqlDataReader rdr = null;
                int val;

                //check for exisitng criteria
                string sql = "empty";
                int Clinic = 0; 
                int Crit = 0;
                if (newCriteria.clinicName != null)
                {
                    Clinic = FindDuplicate(newCriteria.clinicName, "Clinic");
                }
                int Option = FindDuplicate(newCriteria.Value, "Option");
                if (newCriteria.Name != null)
                {
                        Crit = FindDuplicate(newCriteria.Name, "Criteria");
                }
             
                if (mess == "ClinicCriteria")
                {
                    //sql= "IF EXISTS(SELECT * FROM Req WHERE ReqName="+req.Name+") Update Req"
                    //sql = "Insert Into Req Values(@Aug1,@Aug2,@State,@ReqName)";
                    sql = "Select count(1) " +
                            "From ClinicCriteria " +
                           "Join CriteriaOption on CriteriaOption.CriteriaOptionID = ClinicCriteria.Criteria_OptionID " +
                             "join Clinic on Clinic.ClinicID = ClinicCriteria.Clinic_ID " +
                             "join Criteria on ClinicCriteria.Criteria_ID = Criteria.CriteriaID " +
                            "Where Name = @ClinicName and Criteria.CriteriaName = @Name and CriteriaOption.Value = @Value";



                    val = 0;
                
                    cmm = new MySqlCommand(sql, cnn);
                    cmm.Parameters.AddWithValue("@ClinicName", newCriteria.clinicName);
                    cmm.Parameters.AddWithValue("@Name", newCriteria.Name);
                    cmm.Parameters.AddWithValue("@Value", newCriteria.Value);
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
                        //if no match check if there is already a criterian Name
                        sql = "Select count(1) From Criteria Where CriteriaName = @Name";
                        cmm = new MySqlCommand(sql, cnn);                  
                        cmm.Parameters.AddWithValue("@Name", newCriteria.Name);
                        cnn.Open();
                        rdr = cmm.ExecuteReader();
                        while (rdr.Read())
                        {
                            val = Convert.ToInt32(rdr.GetValue(0).ToString());
                        }
                        cnn.Close();
                        if (val > 0)
                        {
                            //it there is check to see if there is a value for it
                            sql = "Select count(1) From CriteriaOption Where Value = @Value";
                            cmm = new MySqlCommand(sql, cnn);
                            cmm.Parameters.AddWithValue("@Value", newCriteria.Value);
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

                               "Insert Into ClinicCriteria(Clinic_ID, Criteria_ID, Criteria_OptionID) Values(@ClinicName,@Crit,@Value); " +
                               "Commit; ";
                                cmm = new MySqlCommand(sql, cnn);
                                cmm.Parameters.AddWithValue("@ClinicName",Clinic);                        
                                cmm.Parameters.AddWithValue("@Value", Option);
                                cnn.Open();
                                cmm.ExecuteNonQuery();
                            }
                            else
                            {

                                sql = "Insert Into CriteriaOption (FK_Criteria_ID,Value) Values(@Crit,@Value);";
                                cmm = new MySqlCommand(sql, cnn);                        
                                cmm.Parameters.AddWithValue("@Crit", Crit);
                                cmm.Parameters.AddWithValue("@Value", newCriteria.Value);
                                cnn.Open();
                                cmm.ExecuteNonQuery();
                               
                                sql = "Insert Into ClinicCriteria(Clinic_ID, Criteria_ID, Criteria_OptionID) Values(@ClinicName,@Crit,@Value);"; 
                                cmm = new MySqlCommand(sql, cnn);
                                cmm.Parameters.AddWithValue("@ClinicName", Clinic);
                                cmm.Parameters.AddWithValue("@Crit", Crit);
                                cmm.Parameters.AddWithValue("@Value", Option);
                                cmm.ExecuteNonQuery();

                            }
                            cnn.Close();

                            Mess = "Criteria Created";
                        }
                        else
                        {

                            sql = "Begin; " +
                                          "Insert Into Criteria (CriteriaName) Values(@Name);" +
                                           "Insert Into CriteriaOption (FK_Criteria_ID,Value) Values(@Crit,@Value);" +
                                          "Commit ;";
                            cmm = new MySqlCommand(sql, cnn);
                            cmm.Parameters.AddWithValue("@Crit", Crit);
                            cmm.Parameters.AddWithValue("@Name", newCriteria.Name);
                            cmm.Parameters.AddWithValue("@Value", newCriteria.Value);
                            cnn.Open();
                            cmm.ExecuteNonQuery();

                            sql = "Insert Into ClinicCriteria(Clinic_ID, Criteria_ID, Criteria_OptionID) Values(@ClinicName,@Crit,@Value);";
                            cmm = new MySqlCommand(sql, cnn);
                            cmm.Parameters.AddWithValue("@ClinicName", Clinic);
                            cmm.Parameters.AddWithValue("@Crit", Crit);
                            cmm.Parameters.AddWithValue("@Value", Option);
                            cmm.ExecuteNonQuery();
                            cnn.Close();
                            Mess = "Criteria Created";
                        }

                    }

                }
                else if (mess == "CriteriaOption")
                {
                    val = 0;
                    //if no match check if there is already a criterian Name
                    sql = "Select count(1) From Criteria Where CriteriaName = @NAme";
                    cmm = new MySqlCommand(sql, cnn);
                    cmm.Parameters.AddWithValue("@Name", newCriteria.Name);
                    cnn.Open();
                    rdr = cmm.ExecuteReader();
                    while (rdr.Read())
                    {
                        val = Convert.ToInt32(rdr.GetValue(0).ToString());
                    }
                    cnn.Close();
                    if (val > 0)
                    {
                        //it there is check to see if there is a value for it
                        sql = "Select count(1 ) From CriteriaOption Where Value = @Value";
                        cmm = new MySqlCommand(sql, cnn);
                        cmm.Parameters.AddWithValue("@Value", newCriteria.Value);
                        cnn.Open();
                        rdr = cmm.ExecuteReader();
                        while (rdr.Read())
                        {
                            val = Convert.ToInt32(rdr.GetValue(0).ToString());
                        }
                        cnn.Close();

                        if (val == 0)
                        {
                            sql = "Begin; " +
                                "Insert Into CriteriaOption (FK_Criteria_ID,Value) Values(@Crit,@Value);"+
                            "Commit ;";
                            cmm = new MySqlCommand(sql, cnn);
                            cmm.Parameters.AddWithValue("@Crit", Crit);
                            cmm.Parameters.AddWithValue("@Value", newCriteria.Value);
                            cnn.Open();
                            cmm.ExecuteNonQuery();
                            Mess = "Criteria Created";

                        }
                        else {
                            Mess = "Criteria Already Exist";
                        }
                    }
                    else
                    {
                        sql = "Begin; " +
                                     "Insert Into Criteria (CriteriaName) Values(@Name);" +
                                    "Insert Into CriteriaOption (FK_Criteria_ID,Value) Values(@Crit,@Value);" +
                                     "Commit ;";
                        cmm = new MySqlCommand(sql, cnn);
                        cmm.Parameters.AddWithValue("@Name", newCriteria.Name);
                        cmm.Parameters.AddWithValue("@Crit", Crit);
                        cmm.Parameters.AddWithValue("@Value", newCriteria.Value);
                        cnn.Open();
                        cmm.ExecuteNonQuery();
                        Mess = "Criteria Created";
                    }

                }
            }
            return Mess;
        }

        public static void DeleteCriteria(int IDvalue, string toDelete)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql = "Empty";

            if (toDelete == "ClinicCriteria")
            {
                sql = "Delete From ClinicCriteria Where ClinicCriteriaID = @IdValue";
            }
            else if(toDelete == "CriteriaOption")
            {
                sql = "Delete From ClinicCriteria Where Criteria_OptionID =@IdValue";
            }
           
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@IdValue", IDvalue);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();


        }
        public static void DeleteCriteriaComplete(int DeleteID)
        {

            DeleteCriteria(DeleteID, "CriteriaOption");
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql;
            sql = "Delete From CriteriaOption Where CriteriaOptionID =@DeleteID";
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@DeleteID", DeleteID);
            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();


        }
        
        private  static int FindDuplicate(string val,string table)
        { int v = 0;
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);


            MySqlDataReader rdr = null;
            string sql = "";

            if (table == "Option")
            {
                sql = "Select CriteriaOptionID From CriteriaOption Where Value =@Val";
            }
            else if (table == "Clinic")
            {
                sql = "Select ClinicID From Clinic Where Name =@Val";
            }
            else if (table == "Criteria")
            {
                sql = "Select CriteriaID From Criteria Where CriteriaName =@Val";
            }
          
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@Val", val);
            cnn.Open();
            rdr = cmm.ExecuteReader();
            while (rdr.Read())
            {
                v = Convert.ToInt32(rdr.GetValue(0).ToString());
            }
            cnn.Close();

            return v;
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
                string sql = "Select ClinicCriteriaID, ClinicCriteria.Clinic, ClinicCriteria.Criteria_Name, CriteriaOption.Value From ClinicCriteria " +
                              "Join CriteriaOption on CriteriaOption.CriteriaOptionID = ClinicCriteria.Criteria_OptionID Where ClinicCriteriaID = @ID";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@ID", ID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int tempID = Convert.ToInt32(rdr.GetValue(0));
                    string clin_name = rdr.GetValue(1).ToString();
                    string crit_Name = rdr.GetValue(2).ToString();
                    string crit_value = rdr.GetValue(3).ToString();
                    clinicReq = new Criteria(tempID, clin_name, crit_Name, crit_value);


         
                }


            }
            cnn.Close();
            return clinicReq;
        }
        public static int GetCriteria(string Name)
        {
            int value = 0;


            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();


            string sql = "Select Value From CriteriaOption where CriteriaOp_Name ='200% Guidelines' ;";

            sql = "Select Value from CriteriaOption " +
            "join Criteria on Criteria.CriteriaID = CriteriaOption.FK_Criteria_ID " +
             "Where CriteriaName ='200% Guidelines' ;";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    value = Int32.Parse(rdr.GetValue(0).ToString());

                }


            
            cnn.Close();

            return value;

        }
        public static List<string> CriteraiValue()
        {
            string[,] list = new string[,] { };
            List<string> ls = new List<string>();
 
            string connString;
            MySqlDataReader rdr = null;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql;
            sql = "Select * from Criteria";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cnn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ls.Add(rdr.GetValue(1).ToString());
            }
            cnn.Close();
  

            return ls;

        }
        public static List<Criteria> GetCriteriaValue()
        {
            string[,] list = new string[,] { };
            List<Criteria> ls = new List<Criteria>();


            string connString;
            MySqlDataReader rdr = null;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql;
            sql = "Select CriteriaOptionID, CriteriaName, Value from CriteriaOption " + 
                "join Criteria on Criteria.CriteriaID = CriteriaOption.FK_Criteria_ID Order by Fk_Criteria_ID;";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cnn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ls.Add(new Criteria
                {
                    CriteriaID = Convert.ToInt32(rdr.GetValue(0).ToString()),
                    Name = rdr.GetValue(1).ToString(),
                    Value = rdr.GetValue(2).ToString()



            }
                );
            }
            cnn.Close();




            return ls;
        }
        public static List<string> GetSpecificCriteira(string specValueNeed)
        {
            List<string> SpecList = new List<string>();
            string connString;
            MySqlDataReader rdr = null;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql;
            sql = "Select Value from CriteriaOption "+
                "join Criteria on Criteria.CriteriaID = CriteriaOption.FK_Criteria_ID "+
                 "Where CriteriaName = '" + specValueNeed + "';";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cnn.Open();
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                SpecList.Add(rdr.GetValue(0).ToString());
            }
            cnn.Close();

            return SpecList;
        }
        public static Criteria GetInCriteria(int ID)
        {
            Criteria editCritera = new Criteria();
            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();
   
                string sql = "Select CriteriaOptionID, CriteriaName, Value from CriteriaOption " +
                            "join Criteria on Criteria.CriteriaID = CriteriaOption.FK_Criteria_ID Where CriteriaOptionID = @ID";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@ID", ID);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    editCritera.CriteriaID = Int32.Parse(rdr.GetValue(0).ToString());
                    editCritera.Name = rdr.GetValue(1).ToString();
                    editCritera.Value = rdr.GetValue(2).ToString();
                


                }


            
            cnn.Close();

            return editCritera;
        }
        public static void EditCriteria(Criteria UpCriteria)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);


            //check for exisitng criteria
            string sql;
            //sql= "IF EXISTS(SELECT * FROM Req WHERE ReqName="+req.Name+") Update Req"
            //sql = "Insert Into Req Values(@Aug1,@Aug2,@State,@ReqName)";
            sql = "Update CriteriaOption Set Value=@Value Where CriteriaOptionID =@ID";


       
            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@ID", UpCriteria.CriteriaID);
       
            cmm.Parameters.AddWithValue("@Value", UpCriteria.Value);
            cnn.Open();
            cmm.ExecuteNonQuery();

            cnn.Close();
            

        }
     
  
    }
}