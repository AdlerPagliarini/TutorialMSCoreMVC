using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorialMSCoreMVC.Models;

namespace TutorialMSCoreMVC.Repositories
{
    public interface IStudentRepository : IDisposable
    {
        IQueryable<Student> GetAll();
        Student GetByID(int id);
        void Insert(Student model);
        void Delete(int id);
        void Update(Student model);
        void Save();
    }
}
