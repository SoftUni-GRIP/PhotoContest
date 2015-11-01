﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoContest.Web.Controllers
{
    using AutoMapper;
    using Data.Contracts;
    using Extensions;
    using Infrastructure.CacheService;
    using Models.ContestModels.ViewModels;
    using Models.UserModels.ViewModels;
    using PhotoContest.Models;

    public class UserController : BaseController
    {
        private ICacheService cache;
        public UserController(IPhotoContestData data, ICacheService cache)
            : base(data)
        {
            this.cache = cache;
        }
        

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = this.Data.Users.Find(id);
            var model = Mapper.Map<User, UserDetails>(user);
            return this.PartialView("_Edit", model);
        }

        [HttpPost]
        public JsonResult Edit(UserDetails model)
        {
            //TODO Validation
            var user = this.Data.Users.Find(model.Id);

            if (user != null)
            {
                this.EditUserData(user, model);
                this.AddNotification("User Edited", NotificationType.SUCCESS);
                return Json(new { Message = "Administration" }, JsonRequestBehavior.AllowGet);
            }

            this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return this.Json(new { Message = "error" }, JsonRequestBehavior.AllowGet);
        }

        private void EditUserData(User user, UserDetails model)
        {

            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            this.Data.Users.Update(user);

            this.Data.SaveChanges();

            this.cache.RemoveContestsFromCache();
        }
    }
}