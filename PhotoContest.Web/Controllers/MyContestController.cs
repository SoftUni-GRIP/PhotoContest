namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Data.Contracts;
    using Models.ContestModels.ViewModels;
    using Infrastructure.CacheService;
    using Infrastructure.Linq;
    using AutoMapper.QueryableExtensions;

    [Authorize]
    public class MyContestController : BaseController
    {
        public MyContestController(IPhotoContestData data) : base(data)
        {
        }

        public ActionResult Index()
        {
            var contests = this.Data.Contests.All().WhereUserIsTheContestOwner()
                                                   .AsQueryable()
                                                   .Project()
                                                   .To<ContestBasicDetails>()
                                                   .ToList();

            return View(CreateHomePageViewModel(contests));
        }

        

        public ActionResult Participated()
        {
            var contests = this.Data.Contests.All().WhereUserIsParticipant()
                                                   .AsQueryable()
                                                   .Project()
                                                   .To<ContestBasicDetails>()
                                                   .ToList();

            return View(CreateHomePageViewModel(contests));
        }

        public ActionResult Voted()
        {
            var contests = this.Data.Contests.All().WhereUserIsVoter()
                                                   .AsQueryable()
                                                   .Project()
                                                   .To<ContestBasicDetails>()
                                                   .ToList();

            return View(CreateHomePageViewModel(contests));
        }

        public ActionResult Won()
        {
            var contests = this.Data.Contests.All().WhereUserIsWinner()
                                                   .AsQueryable()
                                                   .Project()
                                                   .To<ContestBasicDetails>()
                                                   .ToList();

            return View(CreateHomePageViewModel(contests));
        }
    }
}