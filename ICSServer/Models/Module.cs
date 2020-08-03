
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;


namespace ICSServer.Models
{
    public class Module
    {
        [Required]
        public int Id { get; set; }
        //public int SemesterId { get; set; }

        [Display(Name = "Неделя начала")]
        public int WeekOfModuleStart { get; set; }

        [Display(Name = "Неделя окончания")]
        [Remote(action: "VerifyWeekOfModuleEnd", controller: "Modules", HttpMethod = "POST", AdditionalFields = nameof(WeekOfModuleStart), ErrorMessage = "Модуль не может длиться более трёх недель")]
        public int WeekOfModuleEnd { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime  WeekOfModuleStartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        [Remote(action: "VerifyWeekOfModuleEndDate", controller: "Modules", HttpMethod = "POST", AdditionalFields = nameof(WeekOfModuleStart), ErrorMessage = "Модуль не может длиться более трёх недель")]

        public DateTime WeekOfModuleEndDate { get; set; }

       


    }
}
