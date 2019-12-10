using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicV2.Models
{
    public class CreateCriteriaModel
    {
        public List<clinicModel> listofClinic { get; set; }
        public List<Criteria> listofCriteria { get; set; }
        public Criteria Criteria { get; set; }


        

    }

}