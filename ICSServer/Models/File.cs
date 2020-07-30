using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.Models
{
    public class File 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }

        public int Length { get; set; }
    }
}
