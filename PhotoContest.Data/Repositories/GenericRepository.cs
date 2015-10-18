namespace PhotoContest.Data.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private DbContext context;
        private IDbSet<T> set;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public IDbSet<T> Set
        {
            get { return this.set; }
        }

        public IQueryable<T> All()
        {
            return this.Set;
        }

        public T Find(object id)
        {
            return this.Set.Find(id);
        }

        public void Add(T entity)
        {
            this.Set.Add(entity);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void UpdateAsync(T enitity)
        {
            this.ChangeState(enitity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public T Delete(object id)
        {
            var entity = this.Find(id);
            this.Delete(entity);
            return entity;
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}
