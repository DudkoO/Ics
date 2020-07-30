using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class Course
    {
        [Required]
        public int Id { set; get; }


        [Required]
        [Range(1, 6, ErrorMessage = "Курс не может быть меньше 0 или больше 7")]

        [Display(Name = "Номер курса")]
        public int CourseNumber { get; set; }

        public List<Group> Groups { get; set; }


    }
}
