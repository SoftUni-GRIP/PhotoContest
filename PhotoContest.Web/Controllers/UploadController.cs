namespace PhotoContest.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
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
                    Url = Dropbox.Download(paths[0])
                };
                contest.Pictures.Add(photo);
            }

            this.Data.SaveChanges();

            return Content("");
        }
    }
}