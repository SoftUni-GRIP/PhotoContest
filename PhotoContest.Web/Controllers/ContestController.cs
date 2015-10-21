namespace PhotoContest.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using Data.Contracts;
    using Extensions;
    using Models;
    using Models.ContestModels.InputModels;
    using Models.ContestModels.ViewModels;
    using PhotoContest.Models;

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
        public ActionResult Create(ContestInputModel model)
        {
            //TODO: Extract check for model state in a Filter above thee

            if (ModelState.IsValid && model != null)
            {
                var contest = Mapper.Map<ContestInputModel, Contest>(model);
                contest.OwnerId = CurrentUser.Id;

                // TODO: Check for optimization

                foreach (var id in model.UserIds)
                {
                    var user = this.Data.Users.Find(id);
                    contest.Participants.Add(user);
                }

                Data.Contests.Add(contest);
                Data.SaveChanges();

                //TODO: Remove magic strings
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(Contest contest)
        {
            var model = Mapper.Map<Contest, ContestFullDetailsModel>(contest);
            model.CanEdit = this.CanEdit(contest);

            return View(model);
        }

        private bool CanEdit(Contest contest)
        {
            if (CurrentUser == null)
            {
                return false;
            }

            //if (this.User.IsInRole("Administrator"))
            //{
            //    return true;
            //}

            if (contest.OwnerId == this.CurrentUser.Id)
            {
                return true;
            }

            return false;
        }

        [HttpGet]
        public ActionResult Vote(int id)
        {
            //todo validation

            var model = new VoteInput()
            {
                PictureId = id
            };
            return PartialView("_Vote", model);
        }

        [HttpPost]
        public JsonResult Vote(VoteInput model)
        {
            //todo check is allowed to vote on this context with custom filter

            if (this.ModelState.IsValid && model != null)
            {
                var picture = this.Data.Pictures.Find(model.PictureId);

                if (picture != null)
                {
                    var oldVote = picture.Votes.FirstOrDefault(x => x.UserId == this.CurrentUser.Id);
                    if (oldVote != null)
                    {
                        oldVote.Rating = model.Rating;
                    }
                    else
                    {
                        picture.Votes.Add(new Vote()
                        {
                            Rating = model.Rating,
                            UserId = this.CurrentUser.Id
                        });
                    }
                }

                this.Data.SaveChanges();
                this.AddNotification("Vote successfull", NotificationType.SUCCESS);
                var newRating = picture.Votes.Select(x => x.Rating).Average();

                return this.Json(new { stars = newRating });
            }
            
            this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return this.Json("");
        }
    }
}