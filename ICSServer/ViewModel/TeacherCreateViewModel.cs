using ICSServer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.ViewModel
{
    public class TeacherCreateViewModel
    {
        [Required]
        public int Id { set; get; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Имя должно занимать от 2 до 30 символов.")]
        [RegularExpression("[А-ЯЁІ][а-яёі]*")]
        [Display(Name = "Имя")]
        public string Name { set; get; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "Отчество должно занимать от 4 до 30 символов.")]
        [RegularExpression("[А-ЯЁІ][а-яёі]*")]
        [Display(Name = "Отчество")]
        public string Patronymic { set; get; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Фамилия должна занимать от 2 до 30 символов.")]
        [RegularExpression("[А-ЯЁІ][а-яёі]*")]
        [Display(Name = "Фамилия")]
        public string LastName { set; get; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(1000000, MinimumLength = 10, ErrorMessage = "Описание должно занимать от 10 до 500 символов.")]
        [Display(Name = "Описание")]
        public string Description { set; get; }


        [Display(Name = "Академическая степень")]
        public int AcademicDegreesId { get; set; }

        public int TeacherFotoId { get; set; }


        public List<Consultation> Consultations { set; get; }

        public IFormFile Image { get; set; }
    }
}
