using Microsoft.AspNet.Identity.EntityFramework;
using PhotoContest.Data.Contracts;
using PhotoContest.Models;

namespace PhotoContest.Data
{
    public class PhotoContextDbContext : IdentityDbContext<User>, IPhotoDbContext
    {
        public PhotoContextDbContext()
            : base("PhotoContest", throwIfV1Schema: false)
        {
        }

        public static PhotoContextDbContext Create()
        {
            return new PhotoContextDbContext();
        }
    }
}
