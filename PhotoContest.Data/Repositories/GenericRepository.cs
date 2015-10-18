namespace PhotoContest.Data.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            Set = context.Set<T>();
        }

        public IDbSet<T> Set { get; private set; }

        public IQueryable<T> All()
        {
            return Set;
        }

        public T Find(object id)
        {
            return Set.Find(id);
        }

        public void Add(T entity)
        {
            Set.Add(entity);
        }

        public void Update(T entity)
        {
            ChangeState(entity, EntityState.Modified);
        }

        public void UpdateAsync(T enitity)
        {
            ChangeState(enitity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            ChangeState(entity, EntityState.Deleted);
        }

        public T Delete(object id)
        {
            var entity = Find(id);
            Delete(entity);
            return entity;
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Set.Attach(entity);
            }

            entry.State = state;
        }
    }
}