namespace PhotoContest.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Data.Contracts;
    using Microsoft.AspNet.Identity;
    using PhotoContest.Models;

    public class BaseController : Controller
    {
        protected BaseController(IPhotoContestData data)
        {
            Data = data;
        }

        protected IPhotoContestData Data { get; private set; }

        protected User CurrentUser { get; private set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var result = base.BeginExecute(requestContext, callback, state);
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = User.Identity.GetUserId();
                CurrentUser = Data.Users.Find(currentUserId);
            }

            return result;
        }
    }
}