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
        private ICacheService cache;

        public MyContestController(IPhotoContestData data, ICacheService cache) : base(data)
        {
            this.cache = cache;
        }

        public ActionResult Index()
        {
            var contests = this.cache.Contests.WhereUserIsTheContestOwner()
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              .ToList();

            return View(CreateHomePageViewModel(contests));
        }

        

        public ActionResult Participated()
        {
            var contests = this.cache.Contests.WhereUserIsParticipant()
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              .ToList();

            return View(CreateHomePageViewModel(contests));
        }

        public ActionResult Voted()
        {
            var contests = this.cache.Contests.WhereUserIsVoter()
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              .ToList();

            return View(CreateHomePageViewModel(contests));
        }
    }
}