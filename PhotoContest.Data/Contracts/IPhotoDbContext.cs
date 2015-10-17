using System.Threading.Tasks;

namespace PhotoContest.Data.Contracts
{
    public interface IPhotoDbContext
    {
        // TODO: Add IDSet<T>

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
