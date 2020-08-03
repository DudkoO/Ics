using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using System.ComponentModel.DataAnnotations;

namespace ICSServer.API.Public.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly DBContext _context;

        public SemestersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Semesters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semester>>> GetSemesters()
        {
            return await _context.Semesters.ToListAsync();
        }

        // GET: api/Semesters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Semester>> GetSemester(int id)
        {
            var semester = await _context.Semesters.FindAsync(id);

            if (semester == null)
            {
                return NotFound();
            }

            return semester;
        }
        [HttpGet("GetSemesterData")]
        public async Task<ActionResult<Semester>> GetSemesterData()
        {
            DateTime data = DateTime.Now;
            //ищем текущий семестр по дате
            
            List<Semester> semesters = await _context.Semesters.Include(x => x.FirstModule).Include(x => x.SecondModule).Include(x => x.Session).ToListAsync();
            if (semesters == null)
                return NotFound();
            for(int i = 0; i < semesters.Count; i++)
            {
                if (DateTime.Compare(semesters[i].StartDate, data) < 0 && DateTime.Compare(semesters[i].FinishDate, data) > 0)
                {
                    semesters[i].ThisWeek = DefineThisWeek(semesters[i]);
                    return semesters[i];
                }
            }
            

            return NotFound();
            
        }
        public int DefineThisWeek(Semester semester)
        {
            DateTime data = DateTime.Now;
            bool flag = true;
            int counter = 0;
            DateTime firstWeekStartDate;
            if (semester.StartWeek == 1)
                firstWeekStartDate = semester.StartDate;
            else
            {
                int startDay = semester.StartDate.Day;
                firstWeekStartDate = semester.StartDate.AddDays(7 - startDay);
            }
            while (flag)
            {
                if (DateTime.Compare(firstWeekStartDate, data) > 0)
                    return counter;
                else
                {
                    firstWeekStartDate = firstWeekStartDate.AddDays(7);
                    counter++;
                }
                if (counter > semester.Session.WeekOfModuleEnd)
                    flag = false;

            }
            return counter;
        }

        // PUT: api/Semesters/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSemester(int id, Semester semester)
        {
            if (id != semester.Id)
            {
                return BadRequest();
            }

            _context.Entry(semester).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SemesterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Semesters
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Semester>> PostSemester(Semester semester)
        {
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSemester", new { id = semester.Id }, semester);
        }

        // DELETE: api/Semesters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Semester>> DeleteSemester(int id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
            {
                return NotFound();
            }

            _context.Semesters.Remove(semester);
            await _context.SaveChangesAsync();

            return semester;
        }

        private bool SemesterExists(int id)
        {
            return _context.Semesters.Any(e => e.Id == id);
        }
    }
}
