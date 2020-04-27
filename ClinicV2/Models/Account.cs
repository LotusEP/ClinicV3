using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicV2.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Account()
        { }
        public Account(string uName, string pWord)
        {
            Username = uName;
            Password = pWord;
        }
        public Account(string uName, string pWord,string fName, string lName, string email)
        {
            Username = uName;
            Password = pWord;
            FirstName = fName;
            LastName = lName;
            Email = email;
        }

        public static bool login(Account account)
        {
            string connString;
            bool returnCheck = false;
            MySqlConnection cnn;
           
            clinicModel clinic = new clinicModel();
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";

            cnn = new MySqlConnection(connString);

            MySqlDataReader rdr = null;

            cnn.Open();
            string sql = "Select * from AdminUser Where AdminUserName = @Username";

            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@UserName", account.Username);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string userN = rdr.GetValue(1).ToString();
                string passW = rdr.GetValue(2).ToString();
                if (account.Username == userN && account.Password == passW)
                {
                    returnCheck = true;
                }

            }

            return returnCheck;
        }

        public static void Create(Account account)
        {
            string connString;
            MySqlConnection cnn;
            connString = @"Server=clinicsystemdb.cfkpw0ap0abf.us-east-1.rds.amazonaws.com;user id=Lotusep5ep; Pwd=Pat123forsell; database=ClinicSysDB";
            cnn = new MySqlConnection(connString);



            string sql = "Empty";
            sql = "Insert into AdminUser (AdminUserName,AdminPass,FirstName,LastName,Email) Value(@UserName,@Password,@FName,@LName,@Email)";



            MySqlCommand cmm = new MySqlCommand(sql, cnn);
            cmm.Parameters.AddWithValue("@UserName", account.Username.ToString());
            cmm.Parameters.AddWithValue("@Password", account.Password.ToString());
            cmm.Parameters.AddWithValue("@FName", account.FirstName.ToString());
            cmm.Parameters.AddWithValue("@LName", account.LastName.ToString());
            cmm.Parameters.AddWithValue("@Email", account.Email.ToString());

            cnn.Open();
            cmm.ExecuteNonQuery();
            cnn.Close();

        }

    }





 

}