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
    public class NewsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public NewsController(DBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: https://icsserver.conveyor.cloud/api/News/
        [HttpGet("getAllNews")]

        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            List<News> news = new List<News>();
            news = await _context.News.Include(x => x.Publics).OrderBy(s => s.DateOfPublication).ToListAsync();
            return news;
        }

        [HttpGet("getApplicantsNews")]
        public async Task<ActionResult<NewsArrayApiModel>> getApplicantsNews(int offset = 0, int limit = 5)
        {
            DateTime date = DateTime.Now;
            
            NewsArrayApiModel response = new NewsArrayApiModel();
            List<News> news = await _context.News.Where(s=>s.Publics.Applicants).Where(a=>DateTime.Compare( a.DateOfPublication, date)<0).OrderBy(x=>x.DateOfPublication).Skip(offset).Take(limit).ToListAsync();
            response.News = news;
            response.AmountOfNews = news.Count();
            response.TotalAmountOfNews = await _context.News.Where(s => s.Publics.Applicants).Where(a=>DateTime.Compare( a.DateOfPublication, date)<0).CountAsync();
            return response;
        }

        [HttpGet("getStudentsNews")]
        public async Task<ActionResult<NewsArrayApiModel>> getStudentsNews(int offset = 0, int limit = 5)
        {
            DateTime date = DateTime.Now;
            NewsArrayApiModel response = new NewsArrayApiModel();
            List<News> news = await _context.News.Where(s => s.Publics.Students).Where(a => DateTime.Compare(a.DateOfPublication, date) < 0).OrderBy(x => x.DateOfPublication).Skip(offset).Take(limit).ToListAsync();
            response.News = news;
            response.AmountOfNews = news.Count();
            response.TotalAmountOfNews = await _context.News.Where(s => s.Publics.Students).Where(a => DateTime.Compare(a.DateOfPublication, date) < 0).CountAsync();
            return response;
        }
        
        [HttpGet("getGraduatesNews")]
        public async Task<ActionResult<NewsArrayApiModel>> getGraduatesNews(int offset = 0, int limit = 5)
        {
            DateTime date = DateTime.Now;
            NewsArrayApiModel response = new NewsArrayApiModel();
            List<News> news = await _context.News.Where(s => s.Publics.Graduates).Where(a => DateTime.Compare(a.DateOfPublication, date) < 0).OrderBy(x => x.DateOfPublication).Skip(offset).Take(limit).ToListAsync();
            response.News = news;
            response.AmountOfNews = news.Count();
            response.TotalAmountOfNews = await _context.News.Where(s => s.Publics.Graduates).Where(a => DateTime.Compare(a.DateOfPublication, date) < 0).CountAsync();
            return response;
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id, string publics)
        {
            var news = await _context.News.Include(x => x.Publics).FirstAsync(s=>s.Id==id);
            
            if (news == null)
            {             
                    return NotFound();
            }
            //проверяем паблик, чтобы убедиться, что новость принадлежит разделу с которого сделали запрос
            string Applicants = "Applicants";
            string Students = "Students";
            string Graduates = "Graduates";
            if (Applicants.Equals(publics))
            {
                if (!news.Publics.Applicants)
                    return NotFound();
            }
            else if(Students.Equals(publics))
            {
                if (!news.Publics.Students)
                    return NotFound();
            }
            else if (Graduates.Equals(publics))
            {
                if (!news.Publics.Graduates)
                    return NotFound();
            }
            return news;
        }
        [HttpGet("test")]
        public async Task<ActionResult<String>> Test()
        {
            return "Completed";
        }
        


    }
}
