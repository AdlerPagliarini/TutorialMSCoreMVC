using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialMSCoreMVC.Context;
using TutorialMSCoreMVC.Models;

namespace TutorialMSCoreMVC.Repositories
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private SchoolContext context;
        private bool disposed = false;

        public StudentRepository(SchoolContext context)
        {
            this.context = context;
        }

        public void Delete(int id)
        {
            Student student = context.Students.Find(id);
            context.Students.Remove(student);
        }

        public IQueryable<Student> GetAll()
        {
            return context.Students;
        }

        public Student GetByID(int id)
        {
            return context.Students.Find(id);
        }

        public void Insert(Student model)
        {
            context.Students.Add(model);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Student model)
        {
            context.Entry(model).State = EntityState.Modified;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
