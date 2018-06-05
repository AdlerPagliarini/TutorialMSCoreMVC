using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorialMSCoreMVC.Context;
using TutorialMSCoreMVC.Models;
using TutorialMSCoreMVC.Repositories;

namespace TutorialMSCoreMVC.Controllers
{
    public class CoursesUnitOfWorkController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesUnitOfWorkController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private async Task PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAsync(orderBy: q => q.OrderBy(d => d.Name));
            var departmentsQuery = from d in departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            //var courses = _context.Courses.Include(c => c.Department).AsNoTracking();
            var courses = await _unitOfWork.CourseRepository.GetAsync(includeProperties: "Department");
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CourseID == id);*/
            var course = (await _unitOfWork.CourseRepository.GetAsync(c => c.CourseID == id, null, includeProperties: "Department")).SingleOrDefault();
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            ViewData["DepartmentID"] = new SelectList(await _unitOfWork.DepartmentRepository.GetAsync(), "DepartmentID", "Name");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.CourseRepository.InsertAsync(course);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
            await PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _unitOfWork.CourseRepository.GetByIDAsync(id);
                                
            if (course == null)
            {
                return NotFound();
            }
            //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", course.DepartmentID);
            await PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScaffolding(int id, [Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CourseRepository.Update(course);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CourseExists(course.CourseID))
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
            await PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        //Edit from tutorial
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseToUpdate = (await _unitOfWork.CourseRepository.GetAsync(c => c.CourseID == id)).SingleOrDefault();

            if (await TryUpdateModelAsync<Course>(courseToUpdate,
                "",
                c => c.Credits, c => c.DepartmentID, c => c.Title))
            {
                try
                {
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            return View(courseToUpdate);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var course = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.CourseID == id);*/
            var course = await _unitOfWork.CourseRepository.GetAsync(c => c.CourseID == id, null, "Department");
            if (course == null)
            {
                return NotFound();
            }

            return View(course.Single());
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _unitOfWork.CourseRepository.GetByIDAsync(id);
            _unitOfWork.CourseRepository.Delete(course);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CourseExists(int id)
        {
            var course = await _unitOfWork.CourseRepository.GetAsync();
            return course.Any(e => e.CourseID == id);
        }

        public IActionResult UpdateCourseCredits()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourseCredits(int? multiplier)
        {
            if (multiplier != null)
            {
                ViewData["RowsAffected"] =
                    await _unitOfWork.ExecuteSqlCommandAsync("UPDATE Course SET Credits = Credits * {0}", multiplier);
            }
            return View();
        }
    }
}
