using System;

namespace PhotoContest.Data
{
    using System.Data.Entity;
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class PhotoContextDbContext : IdentityDbContext<User>, IPhotoDbContext
    {
        public IDbSet<Contest> Contests { get; set; }

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
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Entity<User>().HasMany(x => x.Contests)
                .WithMany(x => x.Winners);
            modelBuilder.Entity<User>().HasMany(x => x.Contests)
                .WithMany(x => x.Participants);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
