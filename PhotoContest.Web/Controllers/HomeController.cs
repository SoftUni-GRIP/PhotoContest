namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using Data.Contracts;

    public class HomeController : BaseController
    {
        public HomeController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
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