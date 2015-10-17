using System.Threading.Tasks;
using PhotoContest.Models;

namespace PhotoContest.Data.Contracts
{
    public interface IPhotoContestData
    {
        IRepository<User> Users { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
