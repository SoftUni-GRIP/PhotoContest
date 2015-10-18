namespace PhotoContest.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IPhotoDbContext
    {
        // TODO: Add IDSet<T>

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
