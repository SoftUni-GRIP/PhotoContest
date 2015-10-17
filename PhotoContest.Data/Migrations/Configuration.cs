using System.Data.Entity.Migrations;

namespace PhotoContest.Data.Migrations
{
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
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
