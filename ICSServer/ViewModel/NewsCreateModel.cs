using ICSServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.ViewModel
{
    public class NewsCreateModel
    {
        [Required]
        public int Id { set; get; }

        public int PublicsId { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "Имя должно занимать от 4 до 30 символов.")]
        [Display(Name = "Заголовок")]
        public string Title { set; get; }

        [Required]
        //[DataType(DataType.Text)]
        [StringLength(1000000, MinimumLength = 10, ErrorMessage = "Описание должно занимать от 10 до 5000 символов.")]
        // [DisplayFormat(DataFormatString = "")]
        //todo выводить только часть символов
        [Display(Name = "Описание")]
        public string Description { set; get; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Автор занимать от 4 до 40 символов.")]
        [RegularExpression("([А-ЯЁІ][а-яёі]*[ ]?){1,4}")]
        [Display(Name = "Автор")]
        public string Author { set; get; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата публикации")]
        [Remote(action: "VerifyDateOfPublication", controller: "News", HttpMethod = "POST", ErrorMessage = "Дата публикации не может быть старше текущей")]

        public DateTime DateOfPublication { set; get; }

        //todo array file
        
        
        [Display(Name = "Изображение")]
        
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        [Display(Name = "Гиперссылка")]
        [Url(ErrorMessage = "Неверный формат ссылки")]
       

        public string link { set; get; }

        //todo проверка на минимум одно отмеченное поле
        
                 
        public Publics Publics { get; set; }

        public string ErrorMesssage { get; set; } = "1";
        

       
    }

}