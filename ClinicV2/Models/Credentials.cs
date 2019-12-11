using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicV2.Models
{
    public class Credentials
    {
        public string ID { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public List<Credentials> Cred { get; set; }


        public string CompareCredentials(string Username, string Pass)
        {
            List<Credentials> validCredentials = new List<Credentials>();
            MySqlConnection cnn;

            string indicator = "";
            string connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader reader = null;

            cnn.Open();
            string sql = "Select * from Users";

            MySqlCommand command = new MySqlCommand(sql, cnn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                validCredentials.Add(new Credentials
                {
                    ID = reader.GetValue(0).ToString(),

                    UserName = reader.GetValue(1).ToString(),

                    Password = reader.GetValue(2).ToString(),

                    Cred = new List<Credentials>()

                });
            }

            foreach (Credentials CMD in validCredentials)
            {
                if (CMD.UserName != Username && CMD.Password != Pass)
                {
                    indicator = "Login failed"; // Login Failed
                }

                else if (CMD.UserName == Username && CMD.Password != Pass)
                {
                    indicator = "The password you entered is incorrect, please try again."; // Password is incorrect
                }
                else if (CMD.UserName != Username && CMD.Password == Pass)
                {
                    indicator = "Invalid username"; //Username is incorrect 
                }
                else
                {
                    indicator = "Login Successful"; // Login Successful 
                    return indicator;
                }
            }

            return indicator;

        }
    }
}