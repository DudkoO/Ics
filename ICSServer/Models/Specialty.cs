using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class Specialty
    {
        [Required]
        public int Id { set; get; }

        [RegularExpression("А[А-Я]")]
        [Display(Name = "Буквенный код специальности")]
        public string SpecialtyCode { set; get; }

        [Required]
        [Display(Name ="Цифровой код специальности")]
        public int SpecialtyNumberCode { get; set; }
        [Required]
        [Display(Name = "Название специальности")]
        public string Name { get; set; }


    }
}
