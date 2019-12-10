using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicV2.Models
{
    public class SignupModel
    {
        public List<clinicModel> listofClinic { get; set; }
        public Patient newPatient { get; set; }


       
    }
}
