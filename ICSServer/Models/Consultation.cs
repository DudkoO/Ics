using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class Consultation
    {
        [Required]
        public int Id { set; get; }

        [Required]
        public int TeacherId { set; get; }

        [Required]
        [Display(Name = "День недели")]
        public DayOfWeek Day { set; get; }

        [Required]
        [Display(Name = "Аудитория")]
       // [RegularExpression("[1-9][0-9][0-9][Аа-Яя]{0,1}")]
        public string Auditorium { set; get; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "С")]
        public DateTime From { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "По")]
        public DateTime To { get; set; }
    }
}
