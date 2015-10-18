namespace PhotoContest.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Contracts;
    using Models;
    using Repositories;

    public class PhotoContestData : IPhotoContestData
    {
        private readonly IPhotoDbContext context;
        private readonly IDictionary<Type, object> repositories;

        public PhotoContestData(IPhotoDbContext context)
        {
            this.context = context;
            repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get { return GetRepository<User>(); }
        }

        public IRepository<Contest> Contests
        {
            get { return GetRepository<Contest>(); }
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof (T);
            if (!repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof (GenericRepository<T>);
                var repository = Activator.CreateInstance(typeOfRepository, context);
                repositories.Add(type, repository);
            }

            return (IRepository<T>) repositories[type];
        }
    }
}