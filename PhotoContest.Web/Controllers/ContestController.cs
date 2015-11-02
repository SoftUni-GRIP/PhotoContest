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
                contest.OwnerId = this.CurrentUser.Id;

                if (model.UserIds.Count != 0)
                {
                    foreach (var id in model.UserIds)
                    {
                        var user = this.Data.Users.Find(id);
                        contest.Participants.Add(user);
                    }
                }

                if (model.VotersIds.Count != 0)
                {
                    foreach (var id in model.UserIds)
                    {
                        var user = this.Data.Users.Find(id);
                        contest.Voters.Add(user);
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
        //TODO: Filters
        public ActionResult Details(int id)
        {
            var contest = this.Data.Contests.All().Where(x => x.Id == id).Include("Pictures").FirstOrDefault();

            //TODO: Implement Error handling logic
            if (contest == null)
            {
                throw new NotImplementedException();
            }

            var model = Mapper.Map<Contest, ContestFullDetailsModel>(contest);

            if (contest.OwnerId == this.CurrentUser.Id || this.User.IsInRole("Administrator"))
            {
                model.CanEdit = this.CanEdit(contest);    
            }

            if (contest.Status == ContestStatusType.Active && contest.OwnerId != this.CurrentUser.Id)
            {
                if (contest.ParticipationStrategyType == ParticipationStrategyType.Open && contest.MaxNumberOfParticipants > contest.Participants.Count)
                {
                    model.CanParticipate = true;
                }

                if (contest.Participants.Any(u => u.Id == this.CurrentUser.Id))
                {
                    model.CanParticipate = true;
                }

                if (contest.VotingStrategyType == VotingStrategyType.Closed && contest.MaxNumberOfParticipants > contest.Participants.Count && contest.Voters.Any(v => v.Id == this.CurrentUser.Id))
                {
                    model.CanVote = true;
                }

                if (contest.VotingStrategyType == VotingStrategyType.Open)
                {
                    model.CanVote = true;
                }
            }


            return View(model);
        }

        [HttpGet]
        public ActionResult Vote(int id)
        {
            //TODO: Validation

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
            //TODO: check is allowed to vote on this context with custom filter

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

            //this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return this.Json("");
        }

        [HttpGet]
        public ActionResult Delete(Contest contest)
        {
            var model = Mapper.Map<Contest, ContestBasicDetails>(contest);
            return this.PartialView("_Delete", model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var contest = this.Data.Contests.Find(id);
            var model = Mapper.Map<Contest, ContestBasicDetails>(contest);
            return this.PartialView("_Edit", model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(ContestBasicDetails model)
        {
            //TODO Validation
            var contest = this.Data.Contests.Find(model.Id);

            if (contest != null)
            {
                this.EditContestData(contest, model);
                this.AddNotification("Contest Edited", NotificationType.SUCCESS);
                return Json(new { Message = "home" }, JsonRequestBehavior.AllowGet);
            }

            this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return this.Json(new { Message = "error" }, JsonRequestBehavior.AllowGet);
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

        private void EditContestData(Contest contest, ContestBasicDetails model)
        {

            //TODO Validation

            contest.Title = model.Title;
            contest.Description = model.Description;

            this.Data.Contests.Update(contest);

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
        public ActionResult FinalizeViewInvoker(int id)
        {
            var contest = this.Data.Contests.Find(id);
            if (contest != null)
            {
                var model = Mapper.Map<Contest, ContestClosedViewModel>(contest);
                var users = this.Data.Pictures.All().Where(x => x.ContestId == id)
                .Include("Votes").OrderByDescending(x => x.Votes.Average(v => v.Rating))
                .Select(x => x.User.UserName).Take(contest.WinnersCount).ToList();
                //var winners = contest.Pictures
                //        .OrderByDescending(p => p.Votes.Average(v => v.Rating))
                //        .Select(p => p.User.UserName)
                //        
                //        .ToList();
                ////TODO check if no winners

                foreach (var winner in users)
                {
                    model.Winners.Add(winner);
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
        public JsonResult Finalize(int id)
        {
            var contest = this.Data.Contests.Find(id);
            contest.ClosedOn = DateTime.Now;
            contest.Status = ContestStatusType.Finalized;
            var winners = contest.Pictures
                .OrderByDescending(p => p.Votes.Average(v => v.Rating))
                .Select(p => p.User)
                .Take(contest.WinnersCount);

            //TODO check if no winners
            foreach (var winner in winners)
            {
                contest.Winners.Add(winner);
            }

            this.Data.SaveChanges();
            this.cache.RemoveContestsFromCache();
            this.AddNotification("Contest successfully finalized!", NotificationType.SUCCESS);

            return this.Json(new { Message = "Finalized" });
        }

        [HttpGet]
        public ActionResult CreatePrize()
        {
            return this.PartialView("_CreatePrize");
        }
    }
}