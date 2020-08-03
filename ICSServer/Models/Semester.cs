using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class Semester
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Начало семестра")]
        public DateTime StartDate { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Remote(action: "VerifyFinishDate", controller: "Semesters", HttpMethod = "POST", AdditionalFields =nameof(StartDate), ErrorMessage = "Семестр не может длиться более пяти месяцев")]
        [Display(Name = "Конец семестра")]
        public DateTime FinishDate { get; set; }

        [Display(Name ="Номер первой недели")]
        public int StartWeek { get; set; }
        //поле для API
        [NotMapped]
        public int ThisWeek { get; set; }

        [Display(Name = "I Модуль")]
        public Module FirstModule { get; set; }
       // public int FirstModuleId { get; set; }

        [Display(Name = "II Модуль")]
        public Module SecondModule { get; set; }
       // public int SecondModuleId { get; set; }

        [Display(Name ="Сессия")]
        public Module Session { get; set; }
       // public int SessionId { get; set; }
        [Display(Name="Примечание")]
        [StringLength(300,MinimumLength =4, ErrorMessage ="Описание должно занимать от 4 до 300 символов")]
        public string Description { get;set; }

       


    }
}
