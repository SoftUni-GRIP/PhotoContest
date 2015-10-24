namespace PhotoContest.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using Common.Enums;
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

                if (model.UserIds.Count != 0)
                {
                    foreach (var id in model.UserIds)
                    {
                        var user = this.Data.Users.Find(id);
                        contest.Participants.Add(user);
                    }
                }

                foreach (var prize in model.Prizes)
                {
                    var reward = new Reward()
                    {
                        RewardPrice = prize
                    };
                    contest.Rewards.Add(reward);
                }

                this.Data.Contests.Add(contest);

                this.Data.SaveChanges();
                this.cache.RemoveContestsFromCache();
                //TODO: Remove magic strings
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        // this is not working with binding model. Picutes has to be included
        public ActionResult Details(int id)
        {
            var contest = this.Data.Contests
                .All()
                .Where(x => x.Id == id)
                .Include("Pictures").FirstOrDefault();

            if (contest != null)
            {
                var model = Mapper.Map<Contest, ContestFullDetailsModel>(contest);
                model.CanEdit = this.CanEdit(contest);
                return View(model);
            }
            else
            {
                //return view not fond
                throw new NotImplementedException();
            }

        }

        [HttpGet]
        public ActionResult Vote(int id)
        {
            //todo validation

            var model = new VoteInput()
            {
                PictureId = id
            };
            return this.PartialView("_Vote", model);
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

                    this.Data.SaveChanges();
                    this.cache.RemoveContestsFromCache();
                }


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
        [ValidateAntiForgeryToken]
        // TODO validation from admin or 
        public JsonResult Delete(int id)
        {
            //validaton attribute 

            var contest = this.Data.Contests.Find(id);

            if (contest != null)
            {
                this.DeleteContestData(contest);
                this.AddNotification("Contest Deleted", NotificationType.SUCCESS);
                return Json(new { Message = "home" });
            }

            this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return this.Json(new { Message = "error" });

        }

        public ActionResult Edit(int id)
        {
            throw new System.NotImplementedException();
        }

        private bool CanEdit(Contest contest)
        {
            if (this.CurrentUser == null)
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

        private void DeleteContestData(Contest contest)
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
        }

        [HttpGet]
        public ActionResult DissmisViewInvoker(Contest contest)
        {
            // TODO Validation filter

            if (contest != null)
            {
                var model = Mapper.Map<Contest, ContestBasicDetails>(contest);
                return this.PartialView("_DismissContest", model);
            }
            else
            {
                //TODO create not found view
                return this.HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // TODO fix this binding model
        public JsonResult Dismiss(int id)
        {
            var contest = this.Data.Contests.Find(id);
            // TODO add button for activation
            // TODO add button for Finalize
            // TODO add status on details, and update on success
            // TODO validaton filter
            // TODO valddation for null

            contest.Status = ContestStatusType.Dismissed;
            contest.Winners.Clear();
            this.Data.SaveChanges();
            return this.Json(new { Message = "Dissmissed" });
        }

        [HttpGet]
        public ActionResult FinalizeViewInvoker(Contest contest)
        {
            if (contest != null)
            {
                var model = Mapper.Map<Contest, ContestClosedViewModel>(contest);
                var winners = contest.Pictures
                    .OrderByDescending(p => p.Votes)
                    .Select(p => p.User)
                    .Take(contest.WinnersCount);
                foreach (var winner in winners)
                {
                    contest.Winners.Add(winner);
                }

                return this.PartialView("_FinalizeContest", model);
            }
            else
            {
                //TODO create not found view
                return this.HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Finalize(Contest contest)
        {
            //var contest = this.Data.Contests.Find(id);
            contest.ClosedOn = DateTime.Now;
            var winners = contest.Pictures
                .OrderByDescending(p => p.Votes)
                .Select(p => p.User)
                .Take(contest.WinnersCount);

            foreach (var winner in winners)
            {
                contest.Winners.Add(winner);
            }

            this.Data.SaveChanges();
            this.cache.RemoveContestsFromCache();
            return this.Json(new { Message = "Finalized" });
        }

        [HttpGet]
        public ActionResult CreatePrize()
        {
            return this.PartialView("_CreatePrize");
        }
    }
}