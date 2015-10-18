namespace PhotoContest.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PhotoContextDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
#if DEBUG
            this.AutomaticMigrationDataLossAllowed = true;
#endif
        }

        protected override void Seed(PhotoContextDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.SeedRoles(context);
                this.SeedAmin(context);
            }
        }

        private void SeedRoles(PhotoContextDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole { Name = "Administrator" };
            manager.Create(role);
        }

        private void SeedAmin(PhotoContextDbContext context)
        {
            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);
            var admin = new User { UserName = "admin", Email = "admin@abv.bg" };
            manager.Create(admin, "password");
            manager.AddToRole(admin.Id, "Administrator");
        }
    }
}
