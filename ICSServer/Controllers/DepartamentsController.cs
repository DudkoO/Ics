using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using ICSServer.ViewModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ICSServer.Controllers
{
    public class DepartamentsController : Controller
    {
        private readonly DBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DepartamentsController(DBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Departaments
        public async Task<IActionResult> Index()
        {
            List<Departament> departaments = new List<Departament>();
            departaments = await _context.Departaments.Include(x => x.HeadOfDepartment).Include(s => s.Specialty).ToListAsync();
            return View(departaments);
        }

        // GET: Departaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departament = await _context.Departaments.Include(x => x.HeadOfDepartment).Include(s => s.Specialty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departament == null)
            {
                return NotFound();
            }

            return View(departament);
        }

        // GET: Departaments/Create
        public IActionResult Create()
        {
            

            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName");
          
            ViewBag.Specialties = new SelectList(_context.Specialties, "Id", "SpecialtyCode");
            return View();
        }

        // POST: Departaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartamentCreateNewModel model)
        {
            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName");
            ViewBag.Specialties = new SelectList(_context.Specialties, "Id", "SpecialtyCode");



            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Departament departament = new Departament
                {
                    Name = model.Name,                    
                    Description = model.Description,
                    BasicDiscipline = model.BasicDiscipline,
                    RecommendedKnowledge = model.RecommendedKnowledge,
                    Email = model.Email,
                    HeadOfDepartmentId = model.HeadOfDepartmentId,
                    SpecialtyId = model.SpecialtiesId,
                    Image = uniqueFileName,
                    
                };
                _context.Add(departament);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        private string UploadedFile(DepartamentCreateNewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Departaments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName");
            ViewBag.Specialties = new SelectList(_context.Specialties, "Id", "SpecialtyCode");

            var departament = await _context.Departaments.FindAsync(id);
            if (departament == null)
            {
                return NotFound();
            }
            return View(departament);
        }

        // POST: Departaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Description,BasicDiscipline,RecommendedKnowledge,Email,HeadOfDepartmentId,SpecialtyId")] Departament departament, IFormFile newImage)
        {
            if (id != departament.Id)
            {
                return NotFound();
            }
            ViewBag.Teachers = new SelectList(_context.Teachers, "Id", "FullName");
            ViewBag.Specialties = new SelectList(_context.Specialties, "Id", "SpecialtyCode");

            if (ModelState.IsValid)
            {
                //отслеживаем, была ли занружена новая картинка
                if (newImage == null)
                {
                    //находим запись кафедры                        
                    var tss = await _context.Departaments.AsNoTracking().FirstAsync(x => x.Id == departament.Id);
                    //записываем имя старой картинки в кафедру( так как оно не приходит в модели)
                    departament.Image = tss.Image;
                }
                else
                {
                    //находим запись                         
                    var tss = await _context.Departaments.AsNoTracking().FirstAsync(x => x.Id == departament.Id);
                    if (tss.Image != null)
                    {
                        //удаляем файл прежней картинки с диска                            
                        string path = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images", tss.Image);
                        FileInfo oldImage = new FileInfo(path);
                        oldImage.Delete();
                    }
                    //загружаем новую картинку и записываем ее имя в новость. Пау!
                    departament.Image = UploadedFile(new DepartamentCreateNewModel { Image = newImage });
                }
                    try
                    {
                        _context.Update(departament);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DepartamentExists(departament.Id))
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
                return View(departament);
            }
        

            // GET: Departaments/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var departament = await _context.Departaments.Include(x=>x.HeadOfDepartment).Include(s=>s.Specialty)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (departament == null)
                {
                    return NotFound();
                }


                return View(departament);
            }
        
        // POST: Departaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departament = await _context.Departaments.FindAsync(id);
            if (departament.Image != null)
            {
                //удаляем файл прежней картинки с диска                            
                string path = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images", departament.Image);
                FileInfo oldImage = new FileInfo(path);
                oldImage.Delete();
            }
            _context.Departaments.Remove(departament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartamentExists(int id)
        {
            return _context.Departaments.Any(e => e.Id == id);
        }
    }
}
