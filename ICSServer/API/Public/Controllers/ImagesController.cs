using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ICSServer.API.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImagesController(DBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        [HttpGet("{name}")]
        public ActionResult getImage(string name)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images", name);
            try
            {
                Byte[] b = System.IO.File.ReadAllBytes(path);
                return File(b, "image/jpeg");
            }
            catch
            {
                return NotFound();
            }

        }
        [HttpGet("test")]
        public async Task<ActionResult<String>> Test()
        {
            return "Completed";
        }


    }
}
