namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;
    using Data.Contracts;
    using Infrastructure.CacheService;
    using AutoMapper.QueryableExtensions;
    using Models.ContestModels.ViewModels;
    using Models.HomeControllerModels;
    using PagedList;
    using Common;
    using Infrastructure.Linq;

    [Authorize]
    public class MyContestController : BaseController
    {
        private ICacheService cache;

        public MyContestController(IPhotoContestData data, ICacheService cache) : base(data)
        {
            this.cache = cache;
        }

        public ActionResult Index() //int? page
        {
            var contests = this.cache.Contests.WhereUserIsTheContestOwner()
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              //.ToPagedList(page ?? GlobalConstants.DefaultStartingPage, GlobalConstants.DefaultPageSize)
                                              .ToList();

            var model = new HomePageViewModel()
            {
                ContestBasicDetails = contests,
                CurrentUserId = this.CurrentUser == null ? null : this.CurrentUser.Id,
            };

            return View(model);
        }

        public ActionResult Participated() //int? page
        {
            var contests = this.cache.Contests.WhereUserIsParticipant()
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              //.ToPagedList(GlobalConstants.DefaultStartingPage, GlobalConstants.DefaultPageSize)
                                              .ToList();

            var model = new HomePageViewModel()
            {
                ContestBasicDetails = contests,
                CurrentUserId = this.CurrentUser == null ? null : this.CurrentUser.Id,
            };

            return View(model);
        }

        public ActionResult Voted() //int? page
        {
            var contests = this.cache.Contests.WhereUserIsVoter()
                                              .AsQueryable()
                                              .Project()
                                              .To<ContestBasicDetails>()
                                              //.ToPagedList(GlobalConstants.DefaultStartingPage, GlobalConstants.DefaultPageSize)
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