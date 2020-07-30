using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICSServer.Database;
using ICSServer.Models;
using ICSServer.API.Public.Model;

namespace ICSServer.API.Public.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentsController : ControllerBase
    {
        private readonly DBContext _context;

        public DepartamentsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Departaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentShortApiViewModel>>> GetDepartaments()
        {

            List <Departament> departaments= await _context.Departaments.Include(x=>x.HeadOfDepartment).Include(s=>s.Specialty).ToListAsync();
            List<DepartamentShortApiViewModel> response = new List<DepartamentShortApiViewModel>();
            for(int i=0; i < departaments.Count; i++)
            {
                response.Add(new DepartamentShortApiViewModel
                { Id = departaments[i].Id, Name = departaments[i].Name, Image = departaments[i].Image, Description = departaments[i].Description,
                    Email = departaments[i].Email, HeadOfDepartment = departaments[i].HeadOfDepartment.FullName, Specialty = departaments[i].Specialty }
                );
            }

            return response;
            
        }

        // GET: api/Departaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentApiViewModel>> GetDepartament(int id)
        {
            
            Departament departament = await _context.Departaments.FirstAsync(s=>s.Id==id);

            if (departament == null)
            {
                return NotFound();
            }
            departament.HeadOfDepartment = await _context.Teachers.FirstAsync(s=>s.Id==departament.HeadOfDepartmentId);
            departament.Specialty = await _context.Specialties.FirstAsync(s=>s.Id==departament.SpecialtyId);
            DepartamentApiViewModel response = new DepartamentApiViewModel
            {
                Id = departament.Id,
                Name = departament.Name,
                Image = departament.Image,
                Description = departament.Description,
                BasicDiscipline = departament.BasicDiscipline,
                RecommendedKnowledge = departament.RecommendedKnowledge,
                Email = departament.Email,
                HeadOfDepartment = departament.HeadOfDepartment.FullName,
                Specialty = departament.Specialty
            };
            return response;
        }

    }
}
