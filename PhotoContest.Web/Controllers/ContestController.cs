namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using AutoMapper;
    using Data.Contracts;
    using PhotoContest.Models;
    using Models.ContestModels.InputModels;

    [Authorize]
    public class ContestController : BaseController
    {
        public ContestController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateContestInputModel model)
        {
            //TODO: Extract check for model state in a Filter above thee

            if (this.ModelState.IsValid && model != null)
            {
                var contest = Mapper.Map<CreateContestInputModel, Contest>(model);
                contest.OwnerId = this.CurrentUser.Id;

                this.Data.Contests.Add(contest);
                this.Data.SaveChanges();

                //TODO: Remove magic strings
                return this.RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}