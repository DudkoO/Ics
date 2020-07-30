using ICSServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.ViewModel
{
    public class ConsultationsCreateViewModel
    {
        public string ConsultationValidationMessage { get; set; } = "";

        public Consultation Consultation { get; set; } = null;

        
    }
}
