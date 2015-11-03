namespace PhotoContest.Web.Infrastructure.Filters
{
    using System.Web.Mvc;
    using System.Linq;
    using Controllers;
    using Notifications;

    public class MessagesActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check for incoming Toastr objects, in case we've been redirected here
            BaseController controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                controller.Toastr = (controller.TempData["Toastr"] as Toastr) ?? new Toastr();
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            BaseController controller = filterContext.Controller as BaseController;
            if (filterContext.Result.GetType() == typeof(ViewResult))
            {
                if (controller != null)
                {
                    if (controller.Toastr != null && controller.Toastr.ToastMessages.Any())
                    {
                        controller.ViewData["Toastr"] = controller.Toastr;
                    }
                }
            }
            else if (filterContext.Result.GetType() == typeof(RedirectToRouteResult))
            {
                if (controller != null)
                {
                    if (controller.Toastr != null && controller.Toastr.ToastMessages.Any())
                    {
                        controller.TempData["Toastr"] = controller.Toastr;
                    }
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}