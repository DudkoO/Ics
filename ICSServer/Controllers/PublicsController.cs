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
    public class PublicsController : Controller
    {
        private readonly DBContext _context;

        public PublicsController(DBContext context)
        {
            _context = context;
        }
        [HttpPost]
        public JsonResult checkPublics(bool Applicants, bool Students, bool Graduates)
        {
            if (Applicants||Students||Graduates)
            return Json(true);
            return Json(false);

        }
       

        // GET: Publics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Publics.ToListAsync());
        }

        // GET: Publics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publics = await _context.Publics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publics == null)
            {
                return NotFound();
            }

            return View(publics);
        }

        // GET: Publics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NewsId,IsPublic,Students,Graduates,Applicants")] Publics publics)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publics);
        }

        // GET: Publics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publics = await _context.Publics.FindAsync(id);
            if (publics == null)
            {
                return NotFound();
            }
            return View(publics);
        }

        // POST: Publics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NewsId,IsPublic,Students,Graduates,Applicants")] Publics publics)
        {
            if (id != publics.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicsExists(publics.Id))
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
            return View(publics);
        }

        // GET: Publics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publics = await _context.Publics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publics == null)
            {
                return NotFound();
            }

            return View(publics);
        }

        // POST: Publics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publics = await _context.Publics.FindAsync(id);
            _context.Publics.Remove(publics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicsExists(int id)
        {
            return _context.Publics.Any(e => e.Id == id);
        }
    }
}
