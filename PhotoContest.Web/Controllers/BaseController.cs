namespace PhotoContest.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Mvc.Expressions;
    using Data.Contracts;
    using Microsoft.AspNet.Identity;
    using PhotoContest.Models;
    using Common.Enums;
    using Infrastructure.Notifications;

    public class BaseController : Controller
    {
        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
            this.Toastr = new Toastr();
        }

        public Toastr Toastr { get; set; }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {
            return Toastr.AddToastMessage(title, message, toastType);
        }

        protected IPhotoContestData Data { get; private set; }

        protected User CurrentUser { get; private set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var result = base.BeginExecute(requestContext, callback, state);

            if (!User.Identity.IsAuthenticated)
            {
                return result;
            }

            var currentUserId = User.Identity.GetUserId();
            CurrentUser = Data.Users.Find(currentUserId);
            var isAdmin = User.IsInRole("Administrator");

            if(isAdmin)
            {
                this.RedirectToAction<ContestController>(c => c.Create());
            }

            return result;
        }

        protected void DeleteContestData(Contest contest)
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

           

        }

        protected void DeletePicturetData(Picture picture)
        {
            var votes = picture.Votes.ToList();

            for (int i = 0; i < votes.Count; i++)
            {
                var vote = votes[i];
                this.Data.Votes.Delete(vote);
            }

            this.Data.Pictures.Delete(picture);

         

        }
    }
}