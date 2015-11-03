namespace PhotoContest.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.Contracts;
    using Infrastructure.CacheService;
    using Models.AccountModels;
    using Models.ContestModels.ViewModels;
    using Infrastructure.Linq;

    public class HomeController : BaseController
    {
        private ICacheService cache;

        public HomeController(IPhotoContestData data, ICacheService cache) : base(data)
        {
            this.cache = cache;
        }

        public ActionResult Index()
        {
            var contests = this.cache.Contests
                                     .AsQueryable()
                                     .Project()
                                     .To<ContestBasicDetails>()
                                     .ToList();

            return View(CreateHomePageViewModel(contests));
        }

        public ActionResult RenderUsersSearchForm()
        {
            return this.PartialView("_SearchForm");
        }

        public ActionResult SearchUsers(string path)
        {
            var users = this.Data.Users
                                 .All()
                                 .WhereUsernameStartsWith(path)
                                 .Project()
                                 .To<UserSearchViewModel>()
                                 .ToList();
            
            return this.PartialView("_SearchedUsers", users);
        }

        public ActionResult SearchUsersVote(string input)
        {
            var users = this.Data.Users
                                 .All()
                                 .WhereUsernameStartsWith(input)
                                 .Project()
                                 .To<UserSearchViewModel>()
                                 .ToList();

            return this.PartialView("_SearchedUsersVote", users);
        }
    }
}