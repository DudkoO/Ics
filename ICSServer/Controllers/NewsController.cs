using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ICSServer.ViewModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ICSServer.Controllers
{

    [Authorize(Roles = "General, NewsCreator, NewsEditor")]
    public class NewsController : Controller
    {
        private readonly DBContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public NewsController(DBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }


        [HttpPost]
        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyDateOfPublication(DateTime DateOfPublication)
        {
            DateTime date = DateTime.Now;

            if (DateTime.Compare(DateOfPublication, date) < 0)
            {
                return Json(false);
            }

            return Json(true);
        }

        public bool checkFileLenght(IFormFile file)
        {
            int max_size = 2000000;
            if (file.Length > max_size)
                return false;
            return true;
        }
        public bool IsImage(IFormFile file)
        {
            string[] MIMEImageType = { "image/jpeg", "image/pjpeg", "image/jpeg", "image/pjpeg", "image/png" };
            for (int i = 0; i < MIMEImageType.Length; i++)
            {
                if (MIMEImageType[i].Equals(file.ContentType))
                    return true;
            }
            return false;

        }
        public bool IsNotPublics(Publics publics)
        {
            if (publics.Applicants || publics.Students || publics.Graduates)
                return false;
            return true;
        }

        [HttpPost]
        public async Task<JsonResult> filterNewsAsync(string titleFilter, bool filterApplicants, bool filterStudents, bool filterGraduates)
        {
            List<News> news = new List<News>();
            if (String.IsNullOrEmpty(titleFilter))
                news = await _context.News.Include(s => s.Publics).ToListAsync();
            else
                news = await _context.News.Where(s => s.Title.Contains(titleFilter)).Include(x => x.Publics).ToListAsync();

            //todo Вернуть Гитлера абитуриентам
            bool removeFlag = true;

            if ((!filterApplicants && !filterStudents && !filterGraduates) || (filterApplicants && filterStudents && filterGraduates))
                return Json(news);

            else
                for (int i = news.Count - 1; i > -1; i--)
                {
                    if (filterApplicants && news[i].Publics.Applicants)
                        removeFlag = false;
                    else if (filterStudents && news[i].Publics.Students)
                        removeFlag = false;
                    else if (filterGraduates && news[i].Publics.Graduates)
                        removeFlag = false;
                    else removeFlag = true;

                    if (removeFlag)
                        news.Remove(news[i]);
                }
            return Json(news);

        }


        // GET: News

        public async Task<IActionResult> Index()
        {
            List<News> news = new List<News>();
            news = await _context.News.Include(x => x.Publics).OrderBy(s => s.DateOfPublication).ToListAsync();
            return View(news);
        }

        [Authorize(Roles = "General, NewsCreator")]

        public IActionResult New()
        {
            return View(new NewsCreateModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "General, NewsCreator")]
        public async Task<IActionResult> New(NewsCreateModel model)
        {

            //сто тыщь миллионов проверок
            //если не выбрана ни одна аудитория
            if (IsNotPublics(model.Publics)) { model.ErrorMesssage = "Выберите минимум одну аудиторию для публикации"; return View(model); }

            if (model.Image != null)
            {
                //если файл больше 10мб
                if (!checkFileLenght(model.Image)) { model.ErrorMesssage = "К сожалению, ваш файл слишком велик"; return View(model); }
                //если файл - не картинка
                if (!IsImage(model.Image)) { model.ErrorMesssage = "Формат файла не поддерживается. Поддерживаемые форматы: jpg,jpeg,png"; return View(model); }
            }

            if (ModelState.IsValid)
            {

                string uniqueFileName = UploadedFile(model);

                News news = new News
                {
                    Title = model.Title,
                    DateOfPublication = model.DateOfPublication,
                    Description = model.Description,
                    Author = model.Author,
                    link = model.link,
                    Image = uniqueFileName,
                };
                _context.Add(news);
                await _context.SaveChangesAsync();

                Publics Publics = new Publics
                {
                    NewsId = news.Id,
                    Applicants = model.Publics.Applicants,
                    Students = model.Publics.Students,
                    Graduates = model.Publics.Graduates
                };
                _context.Add(Publics);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private string UploadedFile(NewsCreateModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images");
                //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            news.Publics = await _context.Publics.FirstAsync(x => x.Id == news.Id);

            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create



        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }
            news.Publics = await _context.Publics.FirstAsync(x => x.Id == news.Id);
            news.ErrorMessage = "";
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Author,DateOfPublication,link,Students,Graduates,Applicants")]
        News news, IFormFile newImage, bool Applicants, bool Students, bool Graduates)
        {


            if (id != news.Id)
            {
                return NotFound();
            }
            //сто тыщь миллионов проверок
            //если не выбрана ни одна аудитория
            if (IsNotPublics(new Publics { Applicants = Applicants, Students = Students, Graduates = Graduates })) { news.ErrorMessage = "Выберите минимум одну аудиторию для публикации"; return View(news); }

            if (newImage != null)
            {
                //если файл больше 10мб
                if (!checkFileLenght(newImage)) { news.ErrorMessage = "К сожалению, ваш файл слишком велик"; return View(news); }
                //если файл - не картинка
                if (!IsImage(newImage)) { news.ErrorMessage = "Формат файла не поддерживается. Поддерживаемые форматы: jpg,jpeg,png"; return View(news); }
            }


            if (ModelState.IsValid)
            {


                try
                {
                    //нашли паблик, относящийся к этой записи и апгрейднули его
                    var publics = await _context.Publics.FirstAsync(x => x.Id == news.Id);
                    publics.Applicants = Applicants;
                    publics.Students = Students;
                    publics.Graduates = Graduates;
                    _context.Update(publics);
                    await _context.SaveChangesAsync();


                    //отслеживаем, была ли занружена новая картинка
                    if (newImage == null)
                    {
                        //находим запись новости                        
                        var tss = await _context.News.AsNoTracking().FirstAsync(x => x.Id == news.Id);
                        //записываем имя старой картинки в новость( так как оно не приходит в модели)
                        news.Image = tss.Image;
                    }
                    else
                    {
                        //находим запись новости                        
                        var tss = await _context.News.AsNoTracking().FirstAsync(x => x.Id == news.Id);
                        if (tss.Image != null)
                        {
                            //удаляем файл прежней картинки с диска                            
                            string path = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images", tss.Image);
                            FileInfo oldImage = new FileInfo(path);
                            oldImage.Delete();
                        }
                        //загружаем новую картинку и записываем ее имя в новость. Пау!
                        news.Image = UploadedFile(new NewsCreateModel { Image = newImage });
                    }

                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
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
            return View(news);
        }

        [Authorize(Roles = "General, NewsCreator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //todo удаление картинки
            var news = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            news.Publics = await _context.Publics.FirstAsync(x => x.Id == news.Id);

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "General, NewsCreator")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news.Image != null)
            {
                //удаляем файл прежней картинки с диска                            
                string path = Path.Combine(webHostEnvironment.WebRootPath, "Files/Images", news.Image);
                FileInfo oldImage = new FileInfo(path);
                oldImage.Delete();
            }
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}