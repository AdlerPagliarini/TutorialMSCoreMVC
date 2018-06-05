using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialMSCoreMVC.Context;
using TutorialMSCoreMVC.Models;

namespace TutorialMSCoreMVC.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private SchoolContext _context;
        private GenericRepository<Department> departmentRepository;
        private GenericRepository<Course> courseRepository;

        public UnitOfWork(SchoolContext context)
        {
            _context = context;
        }

        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(_context);
                }
                return departmentRepository;
            }
        }

        public GenericRepository<Course> CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                {
                    this.courseRepository = new GenericRepository<Course>(_context);
                }
                return courseRepository;
            }
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<object> ExecuteSqlCommandAsync(string query, int? multiplier)
        {
            return await _context.Database.ExecuteSqlCommandAsync(
                       query,
                       parameters: multiplier);
        }
    }
}
