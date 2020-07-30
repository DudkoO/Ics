using ICSServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.API.Public.Model
{
    public class DepartamentApiViewModel
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        public string Image { get; set; }


        public string Description { get; set; }

        public string BasicDiscipline { get; set; }

        public string RecommendedKnowledge { get; set; }


        public string Email { get; set; }

        public string HeadOfDepartment { get; set; }

        public Specialty Specialty { get; set; }

        
    }
}
