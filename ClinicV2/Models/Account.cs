using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicV2.Models
{
    public class Account
    {
        public int AdminID { get; set; }
        [Required( ErrorMessage = "Invalid/Required UserName" )]
        public string Username { get; set; }
        [Required(ErrorMessage = "Invalid/Required Password")]
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
        public Account(string uName, string pWord, string fName, string lName, string email)
        {
            Username = uName;
            Password = pWord;
            FirstName = fName;
            LastName = lName;
            Email = email;
        }
        public Account(int ID, string uName, string pWord, string fName, string lName, string email)
        {
            AdminID = ID;
            Username = uName;
            Password = pWord;
            FirstName = fName;
            LastName = lName;
            Email = email;
        }

        public static bool login(Account account)
        {

            bool returnCheck = false;


            clinicModel clinic = new clinicModel();
            MySqlConnection cnn = DataModel.getSqlConnection();

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
        [Filters.AuthorizeAdmin]
        public static void Create(Account account)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();

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



        [Filters.AuthorizeAdmin]
        public static List<Account> GetAccountList()
        {
            List<Account> newList = new List<Account>();
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Select * From AdminUser";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cnn.Open();
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                newList.Add(new Account
                {
                    AdminID = Int32.Parse(rdr.GetValue(0).ToString()),
                    Username = rdr.GetValue(1).ToString(),
                    Password = rdr.GetValue(2).ToString(),
                    FirstName = rdr.GetValue(3).ToString(),
                    LastName = rdr.GetValue(4).ToString(),
                    Email = rdr.GetValue(5).ToString()


                });
            }


            cnn.Close();

            return newList;

        }

        [Filters.AuthorizeAdmin]
        public static void DeleteAccount(int ID)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Delete from AdminUser Where AdminID = @ID";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public static Account GetAccount(int ID)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Select * From AdminUser Where AdminID =@ID";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@ID", ID);
            MySqlDataReader rdr = null;

            rdr = cmd.ExecuteReader();

            Account EditAccount = new Account();
            while (rdr.Read())
            {
                EditAccount.AdminID = Int32.Parse(rdr.GetValue(0).ToString());
                EditAccount.Username = rdr.GetValue(1).ToString();
                EditAccount.Password = rdr.GetValue(2).ToString();
                EditAccount.FirstName = rdr.GetValue(3).ToString();
                EditAccount.LastName= rdr.GetValue(4).ToString();

            }
            cnn.Close();

            return EditAccount;

        }
        //update account
        public static void EditAccountInfo(Account acc)
        {
            MySqlConnection cnn = DataModel.getSqlConnection();
            string sql = "Update AdminUser Set AdminUserName =@UserName, AdminPass =@Pass, FirstName =@FName,LastName =@LName, Email =@Email Where AdminID =@ID";
            cnn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@UserName", acc.Username);
            cmd.Parameters.AddWithValue("@Pass", acc.Password);
            cmd.Parameters.AddWithValue("@FName", acc.FirstName);
            cmd.Parameters.AddWithValue("@LName", acc.LastName);
            cmd.Parameters.AddWithValue("@Email", acc.Email);
            cmd.Parameters.AddWithValue("@ID", acc.AdminID);


            cmd.ExecuteNonQuery();
            cnn.Close();

        }


    }





 

}