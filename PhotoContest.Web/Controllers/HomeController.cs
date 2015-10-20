namespace PhotoContest.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.Contracts;
    using Infrastructure.CacheService;
    using Models.AccountModels;
    using Models.ContestModels.ViewModels;
    using Models.HomeControllerModels;

    public class HomeController : BaseController
    {

        private ICacheService cache;

        public HomeController(IPhotoContestData data, ICacheService cache)
            : base(data)
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

            var model = new HomePageViewModel()
            {
                ContestBasicDetails = contests,
                CurrentUserId = this.CurrentUser == null ? null : this.CurrentUser.Id,
               // IsAdmin = this.IsAdmin()
            };

            return View(model);
        }

        //private bool IsAdmin()
        //{
        //   //return Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Owner");;
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RenderUsersSearchForm()
        {
            return this.PartialView("_SearchForm");
        }

        public ActionResult SearchUsers(string path)
        {
            var users = this.Data.Users
                .All()
                .Where(x => x.UserName.StartsWith(path))
                .Project()
                .To<UserSearchViewModel>()
                .ToList();
            return this.PartialView("_SearchedUsers", users);
        }
    }
}