using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.ViewModel
{
    public class SemesterFastCreateModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Начало семестра")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Конец семестра")]
        public DateTime FinishDate { get; set; }
    }
}
