using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ICSServer.ViewModel;

namespace ICSServer.Controllers
{
    public class TeachersController : Controller
    {
        private readonly DBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TeachersController(DBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string searchString)
        {
            List<Teacher> teachers = new List<Teacher>();
            if (!String.IsNullOrEmpty(searchString))
            {
                teachers = await _context.Teachers.Where(s => s.LastName.Contains(searchString)).OrderBy(x => x.LastName).ToListAsync();
            }
            else
                teachers = await _context.Teachers.OrderBy(x=>x.LastName).ToListAsync();
            if (teachers.Count != 0)
            {
                for (int i =0; i < teachers.Count; i++)
                {
                    teachers[i].AcademicDegree = await _context.AcademicDegrees.FirstAsync(s=>s.Id==teachers[i].AcademicDegreesId);
                }
            }

            return View(teachers);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            ViewBag.AcademicDegrees = new SelectList(_context.AcademicDegrees, "Id", "Name");

            return View();
        }
        private string UploadedFile(TeacherCreateViewModel model)
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
        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model,int AcademicDegreesId)
        {
            ViewBag.AcademicDegrees = new SelectList(_context.AcademicDegrees, "Id", "Name");
            //todo писать ид степени

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Teacher teacher = new Teacher
                {
                    Name = model.Name,
                    Patronymic = model.Patronymic,
                    LastName = model.LastName,
                    Description = model.Description,
                    AcademicDegreesId = AcademicDegreesId,                   
                    Image = uniqueFileName,
                    FullName=model.LastName+" "+model.Name+" "+model.Patronymic
                };
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewBag.AcademicDegrees = new SelectList(_context.AcademicDegrees, "Id", "Name");
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Patronymic,LastName,Description,AcademicDegreesId")] Teacher teacher, int AcademicDegreesId, IFormFile newImage)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (newImage == null)
                {
                    //находим запись новости                        
                    var tss = await _context.Teachers.AsNoTracking().FirstAsync(x => x.Id == teacher.Id);
                    //записываем имя старой картинки в новость( так как оно не приходит в модели)
                    teacher.Image = tss.Image;
                }
                else
                {
                    //находим запись новости                        
                    var tss = await _context.Teachers.AsNoTracking().FirstAsync(x => x.Id == teacher.Id);
                    if (tss.Image != null)
                    {
                        //удаляем файл прежней картинки с диска                            
                        string path = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images", tss.Image);
                        FileInfo oldImage = new FileInfo(path);
                        oldImage.Delete();
                    }
                    //загружаем новую картинку и записываем ее имя в новость. Пау!
                    teacher.Image = UploadedFile(new TeacherCreateViewModel { Image = newImage });
                }
                teacher.FullName = teacher.LastName + " " + teacher.Name + " " + teacher.Patronymic;
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher.Image != null)
            {
                //удаляем файл прежней картинки с диска                            
                string path = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images", teacher.Image);
                FileInfo oldImage = new FileInfo(path);
                oldImage.Delete();
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
