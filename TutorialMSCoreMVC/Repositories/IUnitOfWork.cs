using System.Threading.Tasks;
using TutorialMSCoreMVC.Models;

namespace TutorialMSCoreMVC.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<Course> CourseRepository { get; }
        IGenericRepository<Department> DepartmentRepository { get; }

        void Dispose();
        Task SaveAsync();
        Task<object> ExecuteSqlCommandAsync(string query, int? multiplier);
    }
}