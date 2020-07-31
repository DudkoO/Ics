
using System.ComponentModel.DataAnnotations;


namespace ICSServer.Models
{
    public class Publics
    {
        [Required]
        public int Id { get; set; }

        public int NewsId { get; set; }

        

        [Display(Name = "Абитуриентам")]        
        //[Remote(action: "checkPublics", controller: "Publics", HttpMethod = "POST", AdditionalFields =  nameof(Students) + "," + nameof(Graduates),ErrorMessage ="Вы должны выбрать хотя бы одну аудиторию для публикации")]
        public bool Applicants { set; get; }


        
        [Display(Name = "Студентам")]
       // [Remote(action: "checkPublics", controller: "Publics", HttpMethod = "POST", AdditionalFields = nameof(Applicants) + "," + nameof(Graduates), ErrorMessage = "Вы должны выбрать хотя бы одну аудиторию для публикации")]

        public bool Students { set; get; }



        [Display(Name = "Выпусникам")]
        //[Remote(action: "checkPublics", controller: "Publics", HttpMethod = "POST", AdditionalFields = nameof(Applicants) + "," + nameof(Students), ErrorMessage = "Вы должны выбрать хотя бы одну аудиторию для публикации")]
        public bool Graduates { set; get; }


        

        //todo валидация не работает если все флаги fasle
        //каким-то образом, в окне создания (когда модель еще не прошла проверку на валидность), .нет вообще не видит флаги и их значения приходят false, потому пока закоментил
        
    }
}
