namespace PhotoContest.Web.Controllers
{
    using System;
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
    }
}