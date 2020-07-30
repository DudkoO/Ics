using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using ICSServer.Common;
using Microsoft.AspNetCore.Http;
using ICSServer.ViewModel;

namespace ICSServer.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly DBContext _context;

        public ConsultationsController(DBContext context)
        {
            _context = context;
        }

        // GET: Consultations
        public async Task<IActionResult> Index(int? id)
        {
            int TeacherId = id ?? 0;

            if (id != null)
                HttpContext.Session.SetInt32(SessionKeyConstants.SessionKeyTeacherId, (int)id);
            else
                TeacherId = HttpContext.Session.GetInt32(SessionKeyConstants.SessionKeyTeacherId) ?? 0;

            var dBContext = _context.Consultations.Where(x => x.TeacherId == TeacherId).OrderBy(x => (x.From.Minute + x.From.Hour * 60));
            return View(await dBContext.ToListAsync());

           // return View(await _context.Consultation.ToListAsync());
        }

        // GET: Consultations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // GET: Consultations/Create
        public IActionResult Create()
        {
           
            return View(new ConsultationsCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeacherId,Day,Auditorium,From,To")] Consultation consultation)
        {
            int TeacherId = (int)HttpContext.Session.GetInt32(SessionKeyConstants.SessionKeyTeacherId);

            //ViewData["CommandId"] = new SelectList(_context.Commands.Where(x => x.DeviceId == deviceId), "Id", "CommandCode");
            

            if (DateTime.Compare(consultation.From, consultation.To) < 0)
            {

                consultation.TeacherId = TeacherId;
                _context.Add(consultation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(new ConsultationsCreateViewModel()
                {
                    Consultation = consultation,
                    ConsultationValidationMessage =
                            $"Значение поля С должно быть меньше значения поля По"
                });
            }

           
        }
        

        // POST: Consultations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        public async Task<IActionResult> Create([Bind("Id,TeacherId,Day,Auditorium,From,To")] Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consultation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consultation);
        }

        // GET: Consultations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultation.FindAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            return View(consultation);
        }
        */

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherId,Day,Auditorium,From,To")] Consultation consultation)
        {
            if (id != consultation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultationExists(consultation.Id))
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
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            _context.Consultations.Remove(consultation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultationExists(int id)
        {
            return _context.Consultations.Any(e => e.Id == id);
        }
    }
}
