using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using Microsoft.AspNetCore.Authorization;
using ICSServer.ViewModel;

namespace ICSServer.Controllers
{
    [Authorize(Roles = "General, UsersAdmin")]

    public class UsersController : Controller
    {
        private readonly DBContext _context;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.Users.Include(u => u.Role);
            return View(await dBContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(s => s.Name.Equals("General") == false), "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterModel model)
        {

            if (ModelState.IsValid)
            {
                User user = new User { Login = model.Login, Password = model.Password, Description = model.Description, Role = model.Role, RoleId = model.RoleId };
                //хешируем пароль               
                
                string salt = BCrypt.GenerateSalt();
                string HashPassword = BCrypt.HashPassword(user.Password, salt);
                
                user.Password = HashPassword;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(s => s.Name.Equals("General") == false), "Id", "Name", model.RoleId);

            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(s => s.Name.Equals("General") == false), "Id", "Name", user.RoleId);
            


            return View(user);
        }
     

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,Description,RoleId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //если пытаемся изменить роль главного юзера - получаем по башке
                if (user.Login.Equals("general"))
                {
                    if (user.RoleId != 1)
                    {
                        ViewData["RoleId"] = new SelectList(_context.Roles.Where(s => s.Name.Equals("General") == false), "Id", "Name", user.RoleId);
                        return View(user);
                        //todo добавить вывод смс с ошибкой

                    }

                }
                //хешируем пароль               

                string salt = BCrypt.GenerateSalt();
                string HashPassword = BCrypt.HashPassword(user.Password, salt);

                user.Password = HashPassword;
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(s=>s.Name.Equals("General")==false), "Id", "Name", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var user = await _context.Users.FindAsync(id);
            user.Role = await _context.Roles.FirstAsync(x=>x.Id==id);
            //нельзя удалить главную роль

            if (!user.Role.Name.Equals("General"))
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            //todo выводить ошибку
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        
    }
}
