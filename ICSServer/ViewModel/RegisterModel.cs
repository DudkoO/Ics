using ICSServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.ViewModel
{
    public class RegisterModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Не указан Email")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
        public string Description { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
