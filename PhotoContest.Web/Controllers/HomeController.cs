namespace PhotoContest.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.Contracts;
    using Infrastructure.CacheService;
    using Models.ContestModels.ViewModels;

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
            //var contests = this.Data
            //    .Contests
            //    .All()
            //    .Project()
            //    .To<ContestBasicDetails>()
            //    .ToList();

            var contests = this.cache.Contests
                .AsQueryable()
                .Project()
                .To<ContestBasicDetails>()
                .ToList();


            return View(contests);
        }

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
    }
}