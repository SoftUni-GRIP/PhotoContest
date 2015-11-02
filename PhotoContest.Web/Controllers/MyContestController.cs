namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Data.Contracts;
    using Infrastructure.CacheService;
    using AutoMapper.QueryableExtensions;
    using Models.ContestModels.ViewModels;
    using Models.HomeControllerModels;

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
            var contests = this.cache.Contests.Where(c => c.OwnerId == this.CurrentUser.Id)
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              .ToList();

            var model = new HomePageViewModel()
            {
                ContestBasicDetails = contests,
                CurrentUserId = this.CurrentUser == null ? null : this.CurrentUser.Id,
            };

            return View(model);
        }

        public ActionResult Participated()
        {
            var contests = this.cache.Contests.Where(c => c.Participants.Any(p => p.Id == this.CurrentUser.Id))
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              .ToList();

            var model = new HomePageViewModel()
            {
                ContestBasicDetails = contests,
                CurrentUserId = this.CurrentUser == null ? null : this.CurrentUser.Id,
            };

            return View(model);
        }

        public ActionResult Voted()
        {
            var contests = this.cache.Contests.Where(c => c.Voters.Any(v => v.Id == this.CurrentUser.Id))
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              .ToList();

            var model = new HomePageViewModel()
            {
                ContestBasicDetails = contests,
                CurrentUserId = this.CurrentUser == null ? null : this.CurrentUser.Id,
            };

            return View(model);
        }
    }
}