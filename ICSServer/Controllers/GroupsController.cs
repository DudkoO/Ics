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
    public class GroupsController : Controller
    {
        private readonly DBContext _context;

        public GroupsController(DBContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_context.Courses, "Id", "CourseNumber");
            ViewBag.Specialites = new SelectList(_context.Specialties, "Id", "SpecialtyCode");

            return View();
        }
        
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyGroupAsync(int CourseId, int SpecialityId, int Number, string AdditionalFields)
        {

            //if (!_userService.VerifyName(firstName, lastName))
            //{
            //    return Json($"A user named {firstName} {lastName} already exists.");
            //}
            var temp = await _context.Groups.Where(x => x.CourseId == CourseId).Where(s => s.SpecialityId == SpecialityId).Where(r => r.Number == Number).ToListAsync();
            if (temp.Count == 0)
                return Json(true);
            return Json($"Такая группа уже существует");

        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,SpecialityId,Number")] Group group)
        {
            
            if (ModelState.IsValid)
            {
                DateTime date = DateTime.Now;
                
                group.FullName = _context.Specialties.SingleOrDefault(x => x.Id == group.SpecialityId).SpecialtyCode
                    + "-";
                
                if (date.Month <8)
                {
                    group.FullName += date.Year - 2000- _context.Courses.SingleOrDefault(x=>x.Id==group.CourseId).CourseNumber;
                }
                else if(date.Month>=8)
                {
                    group.FullName += date.Year - 2000 - _context.Courses.SingleOrDefault(x => x.Id == group.CourseId).CourseNumber + 1;
                }


                group.FullName += group.Number;

                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }
        

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
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
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
