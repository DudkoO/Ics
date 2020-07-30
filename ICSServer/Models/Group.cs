using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class Group
    {
        [Required]
        public int Id { set; get; }

        [Required]      
        public int CourseId { get; set; }

        [Required]        
        public int SpecialityId { get; set; }


        [Range(1, 9, ErrorMessage = "Номер группы не может быть меньше 0 или больше 9")]
        [Remote(action: "VerifyGroup", controller: "Groups", HttpMethod = "POST", AdditionalFields = nameof(CourseId) +","+nameof(SpecialityId) + "," +nameof(Number))]
        [Display(Name = "Номер группы")]
       
        public int Number { get; set; }

        [Display(Name = "Имя группы")]
       
        public string FullName { get; set; } = "";

        public string TimetableOfClasses { get; set; }
        public string TimetableOfExam { get; set; }
        public string RatingList { get; set; }

        
    }
}
