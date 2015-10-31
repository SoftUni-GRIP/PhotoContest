using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoContest.Web.Controllers
{
    using Data.Contracts;
    using Infrastructure.CacheService;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using PhotoContest.Models;

    public class AdministrationController : BaseController
    {
         private ICacheService cache;
         public AdministrationController(IPhotoContestData data, ICacheService cache)
            : base(data)
        {
            this.cache = cache;
        }
        // GET: Administration
         public ActionResult Index()
         {
             return View();
         }

         public ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
         {
             IQueryable<User> users = this.Data.Users.All();
             DataSourceResult result = users.ToDataSourceResult(request, user => new
             {
                 Id = user.Id,
                 Email = user.Email,
                 EmailConfirmed = user.EmailConfirmed,
                 PasswordHash = user.PasswordHash,
                 SecurityStamp = user.SecurityStamp,
                 PhoneNumber = user.PhoneNumber,
                 PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                 TwoFactorEnabled = user.TwoFactorEnabled,
                 LockoutEndDateUtc = user.LockoutEndDateUtc,
                 LockoutEnabled = user.LockoutEnabled,
                 AccessFailedCount = user.AccessFailedCount,
                 UserName = user.UserName
             });

             return Json(result);
         }

                
    }
}