using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class TeacherOfDepartment
    {
        [Required]
         public int Id { get; set; }
         public int TeaherId { get; set; }
         public int DepartamentId { get; set; }


    }
}
