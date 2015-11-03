namespace PhotoContest.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using Extensions;
    using Models;
    using Models.ContestModels.ViewModels;
    using PhotoContest.Data.Contracts;
    using PhotoContest.Models;
    using PhotoContest.Web.Infrastructure.Dropbox;
    using PhotoContest.Data;

    public class UploadController : BaseController
    {
        public UploadController() : base(new PhotoContestData(new PhotoContextDbContext()))
        {
        }

        public UploadController(IPhotoContestData data)
            : base(data)
        {

        }

        [HttpPost]
        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, int contestId)
        {
            var contest = this.Data.Contests.Find(contestId);

            foreach(var file in files)
            {
                var paths = UploadImages.UploadImage(file, false);
                var photo = new Picture()
                {
                    ContestId = contestId,
                    UserId = this.CurrentUser.Id,
                    CreatedOn = DateTime.Now,
                    Url = Dropbox.Download(paths[0])
                };
                contest.Pictures.Add(photo);
            }

            this.Data.SaveChanges();

            return Content("");
        }

        [HttpGet]
        public ActionResult DeletePicture(int id)
        {
            var picture = this.Data.Pictures.Find(id);
            var model = Mapper.Map<Picture, PictureViewModel>(picture);
           
            return this.PartialView("_DeletePicture", model);
        }

        public JsonResult DeletePicture(PictureViewModel model)
        {
            //TODO Validation
            var picture = this.Data.Pictures.Find(model.Id);

            if (picture != null)
            {
                this.DeletePicturetData(picture);
                this.AddNotification("Contest Edited", NotificationType.SUCCESS);
                return Json(new { Message = "home" }, JsonRequestBehavior.AllowGet);
            }

            this.AddNotification("Something is worng. Plase try again", NotificationType.ERROR);
            return this.Json(new { Message = "error" }, JsonRequestBehavior.AllowGet);
        }

        private void DeletePicturetData(Picture picture)
        {
            var votes = picture.Votes.ToList();

            for (int i = 0; i < votes.Count; i++)
            {
                var vote = votes[i];
                this.Data.Votes.Delete(vote);
            }

            this.Data.Pictures.Delete(picture);

            this.Data.SaveChanges();
        }
    }
}