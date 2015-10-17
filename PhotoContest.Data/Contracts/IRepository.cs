using System.Linq;

namespace PhotoContest.Data.Contracts
{
    public interface IRepository<T>
    {
        IQueryable<T> All();

        T Find(object id);

        void Add(T entity);

        void Update(T entity);

        void UpdateAsync(T entity);

        void Delete(T entity);

        T Delete(object id);
    }
}
