namespace PhotoContest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Common.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PhotoContextDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
#if DEBUG
            AutomaticMigrationDataLossAllowed = true;
#endif
        }

        protected override void Seed(PhotoContextDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.SeedRoles(context);
                this.SeedAmin(context);
            }

            if (!context.Contests.Any())
            {
                this.SeedContests(context);
            }
        }

        private void SeedContests(PhotoContextDbContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                var contest = new Contest()
                {
                    OwnerId = context.Users.First().Id,
                    CreatedOn = DateTime.Now.AddDays(i),
                    Title = "Contest #"+ i,
                    Description = "Desctiption for contest#" + i,
                    MaxNumberOfParticipants = 10,
                    DeadlineDate = DateTime.Now.AddDays(1 + i),
                    Status = ContestStatusType.Active,
                    ParticipationStrategyType = ParticipationStrategyType.Open,
                    VotingStrategyType = VotingStrategyType.Open,
                    WinnersCount = 1
                };

                context.Contests.Add(contest);
            }

            for (int i = 0; i < 10; i++)
            {
                var contest = new Contest()
                {
                    OwnerId = context.Users.First().Id,
                    CreatedOn = DateTime.Now.AddDays((-10 -i)),
                    Title = "Contest #" + i,
                    Description = "Desctiption for contest#" + i,
                    MaxNumberOfParticipants = 10,
                    DeadlineDate = DateTime.Now.AddDays(1 + i),
                    Status = ContestStatusType.Active,
                    ParticipationStrategyType = ParticipationStrategyType.Open,
                    VotingStrategyType = VotingStrategyType.Open,
                    WinnersCount = 1
                };

                context.Contests.Add(contest);
            }

            context.SaveChanges();
        }

        private void SeedRoles(PhotoContextDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole {Name = "Administrator"};
            manager.Create(role);
        }

        private void SeedAmin(PhotoContextDbContext context)
        {
            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);
            var admin = new User {UserName = "admin", Email = "admin@abv.bg"};
            manager.Create(admin, "password");
            manager.AddToRole(admin.Id, "Administrator");
        }
    }
}