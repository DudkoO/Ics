using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class AcademicDegree
    {
        [Required]
        public int Id { set; get; }
        
        [Required]
        [RegularExpression("[А-ЯЁІ][а-яёі]*")]
        [Display(Name = "Именование степени")]
        public string Name { set; get; }

        
    }
}
