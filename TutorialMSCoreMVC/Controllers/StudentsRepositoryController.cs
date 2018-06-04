using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorialMSCoreMVC.Context;
using TutorialMSCoreMVC.Functions;
using TutorialMSCoreMVC.Models;
using TutorialMSCoreMVC.Repositories;

namespace TutorialMSCoreMVC.Controllers
{
    public class StudentsRepositoryController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsRepositoryController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var students = from s in _studentRepository.GetAll()
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            //Changing switch by EF.Property
            //switch (sortOrder)  case "name_desc": case "Date": case "date_desc": default:                
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "LastName";
            }

            bool descending = false;
            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                descending = true;
            }

            if (descending)
            {
                students = students.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }
            else
            {
                students = students.OrderBy(e => EF.Property<object>(e, sortOrder));
            }

            int pageSize = 3;
            return View(await PaginatedList<Student>.CreateAsync(students, page ?? 1, pageSize));
            //return View(await students.AsNoTracking().ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetAll()
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _studentRepository.Insert(student);
                    _studentRepository.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var student = _studentRepository.GetByID(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var studentToUpdate = _studentRepository.GetByID(id);
            if (await TryUpdateModelAsync<Student>(studentToUpdate,"", s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                try
                {
                    _studentRepository.Save();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);

            
        }

        // GET: Students/EditAll/5
        public async Task<IActionResult> EditAll(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var student = _studentRepository.GetByID(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("EditAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(int id, [Bind("ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            /*
             * Use essa abordagem quando a interface do usuário da página da Web incluir todos os 
             * campos na entidade e puder atualizar qualquer um deles.
             */
            if (id != student.ID)
             {
                 return NotFound();
             }
             if (ModelState.IsValid)
             {
                 try
                 {
                     _studentRepository.Update(student);
                     _studentRepository.Save();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!StudentExists(student.ID))
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
             return View(student);
        }

        // GET: Students/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        public async Task<IActionResult> Delete(int id, bool? saveChangesError = false)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var student = _studentRepository.GetByID(id);

            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _studentRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                /*Como excluir sem fazer um select na base de dados, é bom testar e ver se o deleted em cascade esta funcionando*/
                /*Student studentToDelete = new Student() { ID = id };
                _studentRepository.Entry(studentToDelete).State = EntityState.Deleted;
                await _studentRepository.SaveChangesAsync();*/
                _studentRepository.Delete(student.ID);
                _studentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

        }

        private bool StudentExists(int id)
        {
            return _studentRepository.GetAll().Any(e => e.ID == id);
        }
    }
}
