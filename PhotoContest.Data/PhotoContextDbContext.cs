using System.Data.Entity;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.Contests).WithMany(x => x.Winners);
            modelBuilder.Entity<User>().HasMany(x => x.Contests).WithMany(x => x.Participants);
            base.OnModelCreating(modelBuilder);
        }
    }
}
