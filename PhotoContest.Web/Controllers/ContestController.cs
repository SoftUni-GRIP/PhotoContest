namespace PhotoContest.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Expressions;
    using System.Collections.Generic;
    using AutoMapper;
    using Common.Enums;
    using Data.Contracts;
    using Infrastructure.CacheService;
    using Models;
    using Models.ContestModels.InputModels;
    using Models.ContestModels.ViewModels;
    using PhotoContest.Models;
    using Common;
    using Infrastructure.Linq;

    [Authorize]
    public class ContestController : BaseController
    {
        private ICacheService cache;

        public ContestController(IPhotoContestData data, ICacheService cache) : base(data)
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
            if (ModelState.IsValid && model != null)
            {
                var contest = Mapper.Map<ContestInputModel, Contest>(model);
                contest.OwnerId = this.CurrentUser.Id;

                this.AddParticipantsToContest(model, contest);
                this.AddVotersToContest(model, contest);
                this.AddRewardsToContest(model, contest);

                this.Data.Contests.Add(contest);
                this.Data.SaveChanges();
                this.cache.RemoveContestsFromCache();

                return this.RedirectToAction<HomeController>(c => c.Index());
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var contest = this.Data.Contests.All().Where(x => x.Id == id).Include("Pictures").FirstOrDefault();

            if (contest == null)
            {
                return this.HttpNotFound();
            }

            var model = Mapper.Map<Contest, ContestFullDetailsModel>(contest);

            this.CheckIfUserCanEdit(contest, model);
            this.CheckIfUserCanVote(contest, model);
            this.CheckIfUserCanParticipate(contest, model);

            return View(model);
        }

        [HttpGet]
        public ActionResult Vote(int id)
        {
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
            if (this.ModelState.IsValid && model != null)
            {
                var picture = this.Data.Pictures.Find(model.PictureId);

                if (picture != null)
                {
                    this.AddVoteToPicture(model, picture);
                    this.Data.SaveChanges();
                    this.cache.RemoveContestsFromCache();
                }

                this.AddToastMessage(String.Empty, GlobalConstants.VoteSuccessMessage, ToastType.Success);
                var newRating = picture.Votes.Select(x => x.Rating).Average();

                return this.Json(new { stars = newRating });
            }

            this.AddToastMessage(String.Empty, GlobalConstants.SomethingIsWrongMessage, ToastType.Error);

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
            var model = Mapper.Map<Contest, ContestFullDetailsModel>(contest);

            return this.PartialView("_Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest != null)
            {
                this.DeleteContestDataChache(contest);
                this.AddToastMessage(String.Empty, GlobalConstants.ContestDeletedMessage, ToastType.Success);
                return Json(new { Message = GlobalConstants.Home });
            }

            this.AddToastMessage(String.Empty, GlobalConstants.SomethingIsWrongMessage, ToastType.Error);

            return this.Json(new { Message = GlobalConstants.Error });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(ContestFullDetailsModel model)
        {
            var contest = this.Data.Contests.Find(model.Id);

            if (contest != null)
            {
                this.EditContestData(contest, model);
                this.AddToastMessage(String.Empty, GlobalConstants.ContestEditedMessage, ToastType.Success);
                return Json(new { Message = GlobalConstants.Home }, JsonRequestBehavior.AllowGet);
            }

            this.AddToastMessage(String.Empty, GlobalConstants.SomethingIsWrongMessage, ToastType.Error);

            return this.Json(new { Message = GlobalConstants.Error }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult DissmisViewInvoker(Contest contest)
        {
            if (contest == null)
            {
                return this.HttpNotFound();
            }

            var model = Mapper.Map<Contest, ContestBasicDetails>(contest);

            return this.PartialView("_DismissContest", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Dismiss(int id)
        {
            var contest = this.Data.Contests.Find(id);

            this.DismissContest(contest);
            this.Data.SaveChanges();

            return this.Json(new { Message = GlobalConstants.Dissmissed });
        }

        [HttpGet]
        public ActionResult FinalizeViewInvoker(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                return this.HttpNotFound();
            }

            var model = Mapper.Map<Contest, ContestClosedViewModel>(contest);
            var users = this.Data.Pictures.All().SelectWinnersUsernames(contest.WinnersCount, id).ToList();
            this.AddWinnersToContestViewModel(model, users);

            return this.PartialView("_FinalizeContest", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Finalize(int id)
        {
            var contest = this.Data.Contests.Find(id);
            this.FinalizeContest(contest);

            var winners = contest.Pictures.SelectWinners(contest.WinnersCount);
            this.AddWinnersToContest(contest, winners);

            this.Data.SaveChanges();
            this.cache.RemoveContestsFromCache();
            this.AddToastMessage(String.Empty, GlobalConstants.ContestFinalizedMessage, ToastType.Success);

            return this.Json(new { Message = GlobalConstants.Finalized });
        }

        [HttpGet]
        public ActionResult CreatePrize()
        {
            return this.PartialView("_CreatePrize");
        }

        private void AddVoteToPicture(VoteInput model, Picture picture)
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

        private void CheckIfUserCanParticipate(Contest contest, ContestFullDetailsModel model)
        {
            if (contest == null || model == null)
            {
                return;
            }

            if (contest.Status == ContestStatusType.Active && contest.OwnerId != this.CurrentUser.Id)
            {
                if (contest.ParticipationStrategyType == ParticipationStrategyType.Open &&
                    contest.MaxNumberOfParticipants > contest.Participants.Count)
                {
                    model.CanParticipate = true;
                }

                if (contest.Participants.Any(u => u.Id == this.CurrentUser.Id))
                {
                    model.CanParticipate = true;
                }
            }
        }

        private void CheckIfUserCanVote(Contest contest, ContestFullDetailsModel model)
        {
            if (contest.Status == ContestStatusType.Active && contest.OwnerId != this.CurrentUser.Id)
            {
                if (contest.VotingStrategyType == VotingStrategyType.Closed &&
                    contest.MaxNumberOfParticipants > contest.Participants.Count &&
                    contest.Voters.Any(v => v.Id == this.CurrentUser.Id))
                {
                    model.CanVote = true;
                }

                if (contest.VotingStrategyType == VotingStrategyType.Open && contest.OwnerId != this.CurrentUser.Id)
                {
                    model.CanVote = true;
                }
            }
        }

        private void CheckIfUserCanEdit(Contest contest, ContestFullDetailsModel model)
        {
            if (contest.OwnerId == this.CurrentUser.Id || this.User.IsInRole("Administrator"))
            {
                model.CanEdit = this.CanEdit(contest);
            }
        }

        private void AddRewardsToContest(ContestInputModel model, Contest contest)
        {
            if (model == null || contest == null)
            {
                return;
            }

            foreach (var prize in model.Prizes)
            {
                var reward = new Reward()
                {
                    RewardPrice = prize
                };
                contest.Rewards.Add(reward);
            }
        }

        private void AddVotersToContest(ContestInputModel model, Contest contest)
        {
            if (model == null || contest == null)
            {
                return;
            }

            if (model.VotersIds.Count != 0)
            {
                foreach (var id in model.UserIds)
                {
                    var user = this.Data.Users.Find(id);
                    contest.Voters.Add(user);
                }
            }
        }

        private void AddParticipantsToContest(ContestInputModel model, Contest contest)
        {
            if (model == null || contest == null)
            {
                return;
            }

            if (model.UserIds.Count != 0)
            {
                foreach (var id in model.UserIds)
                {
                    var user = this.Data.Users.Find(id);
                    contest.Participants.Add(user);
                }
            }
        }

        private bool CanEdit(Contest contest)
        {
            if (contest == null)
            {
                return false;
            }

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

        private void DeleteContestDataChache(Contest contest)
        {
            if (contest == null)
            {
                return;
            }

            this.DeleteContestData(contest);
            this.Data.SaveChanges();
            this.cache.RemoveContestsFromCache();
        }

        private void EditContestData(Contest contest, ContestFullDetailsModel model)
        {
            if (contest == null || model == null)
            {
                return;
            }

            contest.Title = model.Title;
            contest.Description = model.Description;
            contest.DeadlineDate = model.DeadlineDate;
            contest.MaxNumberOfParticipants = model.MaxNumberOfParticipants;
            contest.ParticipationStrategyType = model.ParticipationStrategyType;
            contest.VotingStrategyType = model.VotingStrategyType;

            this.Data.Contests.Update(contest);
            this.Data.SaveChanges();
            this.cache.RemoveContestsFromCache();
        }

        private void AddWinnersToContest(Contest contest, IEnumerable<User> winners)
        {
            foreach (var winner in winners)
            {
                contest.Winners.Add(winner);
            }
        }

        private void AddWinnersToContestViewModel(ContestClosedViewModel model, List<User> users)
        {
            if (users.Any() && model != null)
            {
                foreach (var winner in users)
                {
                    model.Winners.Add(winner.UserName);
                }
            }
        }

        private void FinalizeContest(Contest contest)
        {
            if (contest != null)
            {
                contest.ClosedOn = DateTime.Now;
                contest.Status = ContestStatusType.Finalized;
            }
        }

        private void DismissContest(Contest contest)
        {
            if (contest != null)
            {
                contest.Status = ContestStatusType.Dismissed;
                contest.Winners.Clear();
            }
        }
    }
}