namespace PhotoContest.Data.Contracts
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Models;

    public interface IPhotoDbContext
    {
        // TODO: Add IDSet<T>

        IDbSet<Contest> Contests { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}