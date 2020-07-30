using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class Image
    {
        [Required]
        public int Id { get; set; }
                
        public string Path { get; set; } = "";

        
    }
}
