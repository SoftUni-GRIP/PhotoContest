using System.Linq;
using System.Web.Mvc;
using PhotoContest.Data.Contracts;

namespace PhotoContest.Web.Controllers
{
    public partial class ValidationController : BaseController
    {
        public ValidationController(IPhotoContestData data)
            : base(data)
        {
        }

        public virtual JsonResult IsUserNameExist(string userName)
        {
            if (userName.Length < 4)
            {
                return this.Json("Username must be minimum 4 characters long", JsonRequestBehavior.AllowGet);
            }

            var currentUserName = this.Data.Users
                .All()
                .Where(x => x.UserName == userName)
                .Select(x => x.UserName)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(currentUserName))
            {
                return this.Json(string.Format("Username {0} is already taken", currentUserName), JsonRequestBehavior.AllowGet);
            }

            return this.Json(true, currentUserName, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult IsEmailExist(string email)
        {
            if (email.Length < 4)
            {
                return this.Json(string.Format("Invalid email"), JsonRequestBehavior.AllowGet);
            }

            var existingEmail = this.Data.Users
                .All()
                .Where(x => x.Email == email)
                .Select(x => x.Email)
                .FirstOrDefault();

            if (existingEmail != null)
            {
                return this.Json(string.Format("Email {0} is already exists", existingEmail), JsonRequestBehavior.AllowGet);
            }

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}