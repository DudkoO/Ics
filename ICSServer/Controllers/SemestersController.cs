using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;

namespace ICSServer.Controllers
{
    public class SemestersController : Controller
    {
        private readonly DBContext _context;

        public SemestersController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyFinishDate(DateTime FinishDate, DateTime StartDate)
        {

            if (DateTime.Compare(FinishDate ,StartDate.AddMonths(5))>0)
            {
                return Json(false);
            }

            return Json(true);
        }
        [HttpPost]
        public async Task<JsonResult> Initial(DateTime StartDate, DateTime FinishDate)
        {

            Semester semester = new Semester();
            semester.StartDate = StartDate;
            semester.FinishDate = FinishDate;
            //вычисляем, летний это семестр или зимний
            //если месяц начала - сентябрь
            if (semester.StartDate.Month == 9)
                semester.Description = "Зимний семестр "+semester.StartDate.Year+" года";
            else
                semester.Description = "Летний семестр " + semester.StartDate.Year + " года";

            //рассчитываем номер первой недели
            if (semester.StartDate.DayOfWeek.ToString().Equals("Monday"))
                semester.StartWeek = 1;
            else semester.StartWeek = 0;
            //вычисляем дату начала первой недели
            DateTime firstWeekStartDate;
            if (semester.StartWeek == 1)
                firstWeekStartDate = semester.StartDate;
            else
            {
                int startDay = semester.StartDate.Day;
                firstWeekStartDate = semester.StartDate.AddDays(7 - startDay);
            }
            //вычисляем данные первого модуля
            semester.FirstModule = new Module();
            semester.FirstModule.WeekOfModuleStart = 6;
            semester.FirstModule.WeekOfModuleEnd = 8;
            semester.FirstModule.WeekOfModuleStartDate = firstWeekStartDate.AddDays(semester.FirstModule.WeekOfModuleStart * 7).AddDays(-7);
            semester.FirstModule.WeekOfModuleEndDate = semester.FirstModule.WeekOfModuleStartDate.AddDays(11);
            //вычисляем данные второго модуля 
            semester.SecondModule = new Module();
            semester.SecondModule.WeekOfModuleStart = 12;
            semester.SecondModule.WeekOfModuleEnd = 14;
            semester.SecondModule.WeekOfModuleStartDate = firstWeekStartDate.AddDays(semester.SecondModule.WeekOfModuleStart * 7).AddDays(-7);
            semester.SecondModule.WeekOfModuleEndDate = semester.SecondModule.WeekOfModuleStartDate.AddDays(11);

            //вычисляем данные сессии
            semester.Session = new Module();
            semester.Session.WeekOfModuleStart = 14;
            semester.Session.WeekOfModuleEnd = 16;
            semester.Session.WeekOfModuleStartDate = firstWeekStartDate.AddDays(semester.Session.WeekOfModuleStart * 7).AddDays(-7);
            semester.Session.WeekOfModuleEndDate = semester.Session.WeekOfModuleStartDate.AddDays(11);


            return Json(semester);
        }

      

        // GET: Semesters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Semesters.ToListAsync());
        }

        // GET: Semesters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semester == null)
            {
                return NotFound();
            }

            return View(semester);
        }

        // GET: Semesters/Create
        public IActionResult Create()
        {
            

            return View(new Semester());
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Semester semester)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(semester);
                await _context.SaveChangesAsync();

                //semester.FirstModule = new Module
                //{
                //    SemesterId = semester.Id,
                //    WeekOfModuleStart = semester.FirstModule.WeekOfModuleStart,
                //    WeekOfModuleEnd=semester.FirstModule.WeekOfModuleEnd,
                //    WeekOfModuleStartDate=semester.FirstModule.

                //}
                semester.FirstModule.Id = semester.Id;
                _context.Add(semester.FirstModule);

                return RedirectToAction(nameof(Index));
            }
            return View(semester);
        }

        // GET: Semesters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
            {
                return NotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,FinishDate,FirstModuleId,SecondModuleId,SessionId,Description")] Semester semester)
        {
            if (id != semester.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semester);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemesterExists(semester.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(semester);
        }

        // GET: Semesters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (semester == null)
            {
                return NotFound();
            }

            return View(semester);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            _context.Semesters.Remove(semester);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SemesterExists(int id)
        {
            return _context.Semesters.Any(e => e.Id == id);
        }
    }
}
