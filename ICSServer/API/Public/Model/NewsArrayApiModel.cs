using ICSServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICSServer.API.Public
{
    public class NewsArrayApiModel
    {
        public int AmountOfNews { get; set; }

        public int TotalAmountOfNews { get; set; }
        public List<News> News { get; set; }
    }
}
