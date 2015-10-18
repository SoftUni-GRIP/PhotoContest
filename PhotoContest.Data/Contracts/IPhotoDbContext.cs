using System.Data.Entity;
using PhotoContest.Models;

namespace PhotoContest.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IPhotoDbContext
    {
        // TODO: Add IDSet<T>

        IDbSet<Contest> Contests { get; set; } 

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
