namespace Contests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using PhotoContest.Common.Enums;
    using PhotoContest.Data.Contracts;
    using PhotoContest.Models;

    public class MockRepository
    {
        public Mock<IRepository<Contest>> ContestRepositoryMock { get; set; }

        public Mock<IRepository<User>> UserRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetUpFakeContests();
            this.SetUpFakeUsers();
        }

        private void SetUpFakeUsers()
        {
            var fakeUsers = this.AddFakeUsers();

            this.UserRepositoryMock = new Mock<IRepository<User>>();
            this.UserRepositoryMock.Setup(u => u.All())
                .Returns(fakeUsers.AsQueryable());
            this.UserRepositoryMock.Setup(u => u.Find(It.IsAny<string>()))
                .Returns((string id) =>
                {
                    var user = fakeUsers.FirstOrDefault(u => u.Id == id);

                    return user;
                });
        }

        private void SetUpFakeContests()
        {
            var fakeContests = this.AddFakeContests();

            this.ContestRepositoryMock = new Mock<IRepository<Contest>>();
            this.ContestRepositoryMock.Setup(c => c.All())
                .Returns(fakeContests.AsQueryable());
            this.ContestRepositoryMock.Setup(c => c.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var contest = fakeContests.FirstOrDefault(c => c.Id == id);

                    return contest;
                });
        }


        private List<Contest> AddFakeContests()
        {
            var users = this.AddFakeUsers();

            var contests = new List<Contest>
            {
                new Contest
                {
                    OwnerId = users.First().Id,
                    CreatedOn = DateTime.Now,
                    Title = "Contest #" + 1,
                    Description = "Desctiption for contest #" + 1,
                    MaxNumberOfParticipants = 10,
                    DeadlineDate = DateTime.Now.AddDays(1),
                    Status = ContestStatusType.Active,
                    ParticipationStrategyType = ParticipationStrategyType.Open,
                    VotingStrategyType = VotingStrategyType.Open,
                    WinnersCount = 1
                    
                },
                 new Contest
                {
                    OwnerId = users.First().Id,
                    CreatedOn = DateTime.Now,
                    Title = "Contest #" + 2,
                    Description = "Desctiption for contest #" + 2,
                    MaxNumberOfParticipants = 10,
                    DeadlineDate = DateTime.Now.AddDays(1),
                    Status = ContestStatusType.Active,
                    ParticipationStrategyType = ParticipationStrategyType.Open,
                    VotingStrategyType = VotingStrategyType.Open,
                    WinnersCount = 1
                }
            };
            contests[1].Pictures.Add(new Picture()
            {
                CreatedOn = DateTime.Now,
                User = users.First(),
                Url = "http://vignette1.wikia.nocookie.net/josh100lubu/images/4/40/18360-doge-doge-simple.jpg/revision/latest?cb=20150626051745",
                Votes = new HashSet<Vote>()
                    {
                        new Vote()
                        {
                            User = users.First(),
                            Rating = 5,
                            CreatedOn = DateTime.Now
                        }
                    }
            });

            contests[1].Pictures.Add(new Picture()
            {
                CreatedOn = DateTime.Now,
                User = users.First(),
                Url = "http://vignette1.wikia.nocookie.net/josh100lubu/images/4/40/18360-doge-doge-simple.jpg/revision/latest?cb=20150626051745",
                Votes = new HashSet<Vote>()
                    {
                        new Vote()
                        {
                            User = users.First(),
                            Rating = 5,
                            CreatedOn = DateTime.Now
                        }
                    }
            });

            
            return contests;
        }

        private List<User> AddFakeUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    UserName = "nikola",
                    Id = "1"
                },
                new User()
                {
                    UserName = "radoslav",
                    Id = "2"
                },
                new User()
                {
                    UserName = "martin",
                    Id = "3"
                }
            };

            return users;
        }
    }
}