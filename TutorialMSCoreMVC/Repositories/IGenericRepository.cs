using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TutorialMSCoreMVC.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task DeleteAsync(object id);
        void Delete(TEntity entityToDelete);

        //Expression<Func<TEntity, bool>> filter = null (student => student.LastName == "Adler")
        //Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null (q => q.OrderBy(s => s.LastName))
        Task <IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task <TEntity> GetByIDAsync(object id);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}