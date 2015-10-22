namespace PhotoContest.Web.Controllers
{
    using System.Linq;
    using System.Net.Sockets;
    using System.Web.Mvc;
    using System.Web.Security;
    using AutoMapper;
    using Data.Contracts;
    using Extensions;
    using Infrastructure.CacheService;
    using Models;
    using Models.ContestModels.InputModels;
    using Models.ContestModels.ViewModels;
    using PhotoContest.Models;


    [Authorize]
    public class ContestController : BaseController
    {

        private ICacheService cache;
        public ContestController(IPhotoContestData data, ICacheService cache)
            : base(data)
        {
            this.cache = cache;
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
        [ValidateAntiForgeryToken]
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
                //this.AddNotification("Vote successfull", NotificationType.SUCCESS);
                var newRating = picture.Votes.Select(x => x.Rating).Average();

                return this.Json(new { stars = newRating });
            }

            this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return this.Json("");
        }

        [HttpGet]
        public ActionResult Delete(Contest contest)
        {
            var model = Mapper.Map<Contest, ContestBasicDetails>(contest);
            return this.PartialView("_Delete", model);
        }

        [HttpPost]
        // TODO validation from admin or 
        public JsonResult Delete(int id)
        {
            //validaton attribute 

            var contest = this.Data.Contests.Find(id);

            if (contest != null)
            {

                var pictures = contest.Pictures.ToList();

                for (int i = 0; i < pictures.Count; i++)
                {
                    var picture = pictures[i];
                    var votes = picture.Votes.ToList();
                    for (int j = 0; j < votes.Count; j++)
                    {
                        var vote = votes[j];
                        this.Data.Votes.Delete(vote);
                    }

                    this.Data.Pictures.Delete(picture);
                }

                this.Data.Contests.Delete(contest);

                this.Data.SaveChanges();

                this.cache.RemoveContestsFromCache();
                this.AddNotification("Contest Deleted", NotificationType.SUCCESS);
                return Json(new {Message = "home"});
            }

            this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return Json(new{Message = "error"});

        }

        private bool CanEdit(Contest contest)
        {
            if (CurrentUser == null)
            {
                return false;
            }

            if (this.User.IsInRole("Administrator"))
            {
                return true;
            }

            if (contest.OwnerId == this.CurrentUser.Id)
            {
                return true;
            }

            return false;
        }

        public ActionResult Edit(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}