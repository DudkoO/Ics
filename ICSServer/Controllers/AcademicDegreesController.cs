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
    public class AcademicDegreesController : Controller
    {
        private readonly DBContext _context;

        public AcademicDegreesController(DBContext context)
        {
            _context = context;
        }

        // GET: AcademicDegrees
        public async Task<IActionResult> Index()
        {
            return View(await _context.AcademicDegrees.ToListAsync());
        }

        // GET: AcademicDegrees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicDegree = await _context.AcademicDegrees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicDegree == null)
            {
                return NotFound();
            }

            return View(academicDegree);
        }

        // GET: AcademicDegrees/Create
        public IActionResult Create()
        {
            ViewBag.AcademicDegrees = new SelectList(_context.AcademicDegrees, "Id", "Name");


            return View();
        }

        // POST: AcademicDegrees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] AcademicDegree academicDegree)
        {


            if (ModelState.IsValid)
            {
                _context.Add(academicDegree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academicDegree);
        }

        // GET: AcademicDegrees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicDegree = await _context.AcademicDegrees.FindAsync(id);
            if (academicDegree == null)
            {
                return NotFound();
            }
            return View(academicDegree);
        }

        // POST: AcademicDegrees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherId,Name")] AcademicDegree academicDegree)
        {
            if (id != academicDegree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicDegree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicDegreeExists(academicDegree.Id))
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
            return View(academicDegree);
        }

        // GET: AcademicDegrees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicDegree = await _context.AcademicDegrees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicDegree == null)
            {
                return NotFound();
            }

            return View(academicDegree);
        }

        // POST: AcademicDegrees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academicDegree = await _context.AcademicDegrees.FindAsync(id);
            _context.AcademicDegrees.Remove(academicDegree);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicDegreeExists(int id)
        {
            return _context.AcademicDegrees.Any(e => e.Id == id);
        }
    }
}
