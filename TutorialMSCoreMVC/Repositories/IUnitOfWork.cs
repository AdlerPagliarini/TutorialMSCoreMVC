using System.Threading.Tasks;
using TutorialMSCoreMVC.Models;

namespace TutorialMSCoreMVC.Repositories
{
    public interface IUnitOfWork
    {
        GenericRepository<Course> CourseRepository { get; }
        GenericRepository<Department> DepartmentRepository { get; }

        void Dispose();
        Task SaveAsync();
        Task<object> ExecuteSqlCommandAsync(string query, int? multiplier);
    }
}