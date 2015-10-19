﻿namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using AutoMapper;
    using Data.Contracts;
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
    }
}