namespace Contests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoMapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PhotoContest.Common.Enums;
    using PhotoContest.Data.Contracts;
    using PhotoContest.Models;
    using PhotoContest.Web;
    using PhotoContest.Web.Controllers;
    using PhotoContest.Web.Models.AccountModels;
    using PhotoContest.Web.Models.ContestModels.InputModels;
    using PhotoContest.Web.Models.ContestModels.ViewModels;

    [TestClass]
    public class ContestControllerTests
    {
        private MockRepository mocks;

        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockRepository();
            this.mocks.PrepareMocks();
        }

        [TestMethod]
        public void CreateContest_WithValidData_ShouldSuccessfullyAddTheContest()
        {
            var contests = new List<Contest>();
            var organizator = this.mocks
                .UserRepositoryMock
                .Object
                .All()
                .FirstOrDefault();
            if (organizator == null)
            {
                Assert.Fail("Cannot perform test - no users available");
            }

            this.mocks.ContestRepositoryMock.Setup(c => c.Add(It.IsAny<Contest>()))
                .Callback((Contest contest) =>
                {
                    contest.Owner = organizator;
                    contests.Add(contest);
                });

            var mockContext = new Mock<IPhotoContestData>();
            mockContext.Setup(c => c.Contests)
                .Returns(this.mocks.ContestRepositoryMock.Object);
            mockContext.Setup(c => c.Users)
                .Returns(this.mocks.UserRepositoryMock.Object);

            

            var contestController = new ContestController(mockContext.Object,null);

            string contestTitle = "Title";
            contests.AddRange(mocks.ContestRepositoryMock.Object.All().ToList());

            var newContest = new ContestInputModel
            {
                Title = contestTitle,
                Description = "Test contest",
                MaxNumberOfParticipants = 10,
                DeadlineDate = DateTime.Now.AddDays(1),
                Status = ContestStatusType.Active,
                ParticipationStrategyType = ParticipationStrategyType.Open,
                VotingStrategyType = VotingStrategyType.Open,
                WinnersCount = 1
            };

            newContest.Prizes.Add(10);

            

            Mapper.CreateMap<ContestInputModel, Contest>();
            ActionResult response = contestController.Create(newContest);

            var contestFromRepo = contests.FirstOrDefault(c => c.Title == newContest.Title);
            if (contestFromRepo == null)
            {
                Assert.Fail();
            }

            Assert.AreEqual(1, contests.Count);
            Assert.AreEqual(newContest.Description, contestFromRepo.Description);
        }

        [TestMethod]
        public void TestDetailsView()
        {
            var mockContext = new Mock<IPhotoContestData>();
            mockContext.Setup(c => c.Contests)
                .Returns(this.mocks.ContestRepositoryMock.Object);
            mockContext.Setup(c => c.Users)
                .Returns(this.mocks.UserRepositoryMock.Object);
            Mapper.CreateMap<Contest, ContestFullDetailsModel>();



            var controller = new HomeController(mockContext.Object, null);
            
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);

        }
    }
}
