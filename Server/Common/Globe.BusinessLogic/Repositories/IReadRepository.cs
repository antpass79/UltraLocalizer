using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Globe.BusinessLogic.Repositories
{
    public interface IReadRepository<T, in K>
        where T: class
    {
        IEnumerable<T> Get(
                    Expression<Func<T, bool>> filter = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        T FindById(K id);
    }

    public interface IReadRepository<T> : IReadRepository<T, Guid>
        where T : class
    {
    }

    public interface IAsyncReadRepository<T, in K>
        where T : class
    {
        Task<IEnumerable<T>> GetAsync(
                    Expression<Func<T, bool>> filter = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<T> FindByIdAsync(K id);
    }

    public interface IAsyncReadRepository<T> : IAsyncReadRepository<T, Guid>
        where T : class
    {
    }
}
