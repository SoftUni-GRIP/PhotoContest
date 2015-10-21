namespace PhotoContest.Data.Contracts
{
    using System.Threading.Tasks;
    using Models;

    public interface IPhotoContestData
    {
        IRepository<User> Users { get; }

        IRepository<Contest> Contests { get; }

        IRepository<Picture> Pictures { get; }

        IRepository<Vote> Votes { get; } 

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}