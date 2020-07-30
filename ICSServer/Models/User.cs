using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
               
       
        public string Description { get; set; }

        
        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
