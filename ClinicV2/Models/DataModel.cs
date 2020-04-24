using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ClinicV2.Models
{
    public class DataModel
    {
        public string DateName { get; set; }
        public int NumDataValue { get; set; }
        public string DataValue { get; set; }

        public DataModel()
        { }

        public DataModel(string Name, int Value, string sValue)
        {
            DateName = Name;
            NumDataValue = Value;
            DataValue = sValue;
        }
        public DataModel(string Name, int Value)
        {
            DateName = Name;
            NumDataValue = Value;
          
        }
        public static List<DataModel> Source()
        {

            List<DataModel> ArraySets = DataModel.getTrafficinfo();
            List<DataModel> TrafficInfo = new List<DataModel>();

            for (int x = 0; x < ArraySets.Count; x++)
            {
                if (TrafficInfo.Count == 0)
                {
                    TrafficInfo.Add(new DataModel(ArraySets[0].DateName, 1));

                }
                else
                {
                    int addCounter = 0;
                    for (int y = 0; y < TrafficInfo.Count; y++)
                    {
                        if (ArraySets[x].DateName == TrafficInfo[y].DateName)
                        {
                            TrafficInfo[y].NumDataValue += 1;
                            addCounter = 1;
                        }                     

                    }
                    if (addCounter == 0)
                    {
                        TrafficInfo.Add(new DataModel(ArraySets[x].DateName, 1, ArraySets[x].DataValue));
                    }

                }

            }
         


            return TrafficInfo;

        }
        public static List<DataModel> getTrafficinfo()
        {
            List<DataModel> TrafficInfo = new List<DataModel>();


            string connString;
            MySqlConnection cnn;
            //connString = @"Data Source=clinicserver1.database.windows.net;Initial Catalog=Patient;User ID=Lotus;Password=Server1@pass;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();

                string sql = "Select * from DestinationSource";

                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    TrafficInfo.Add(new DataModel
                    {
                        DateName = rdr.GetValue(1).ToString(),
                        DataValue = rdr.GetValue(2).ToString()
                        

                    });
                }


            return TrafficInfo;

        }
    }
}