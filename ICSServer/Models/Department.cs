using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class Departament
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }

        
        public string Description { get; set; }

        public string BasicDiscipline { get; set; }

        public string RecommendedKnowledge { get; set; }


        public string Email { get; set; }

        public int HeadOfDepartmentId { get; set; }

        public int SpecialtyId { get; set; }

        public Specialty Specialty { get; set; }

        public Teacher HeadOfDepartment { get; set; }

        public List<Teacher> Teachers { get; set; }
    }
}
