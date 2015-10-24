namespace PhotoContest.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Contracts;

    public class PhotoContextDbContext : IdentityDbContext<User>, IPhotoDbContext
    {
        public PhotoContextDbContext()
            : base("PhotoContest", false)
        {

        }

        public IDbSet<Contest> Contests { get; set; }

        public static PhotoContextDbContext Create()
        {
            return new PhotoContextDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<User>()
                .HasMany(x => x.Contests)
                .WithMany(x => x.Participants);

            modelBuilder.Entity<User>()
                .HasMany(x => x.WonContests)
                .WithMany(x => x.Winners)
                .Map(m =>
                {
                    m.ToTable("UsersWonContests");
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Contest_Id");
                });

            base.OnModelCreating(modelBuilder);
        }

        private void ApplyCreationDateRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IHaveCreationDate && (e.State == EntityState.Added));
            foreach (var entry in entries)
            {
                var entity = (IHaveCreationDate) entry.Entity;
                entity.CreatedOn = DateTime.Now;
            }

        }
    }
}