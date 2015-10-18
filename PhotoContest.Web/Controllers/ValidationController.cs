namespace PhotoContest.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Data.Contracts;

    public class ValidationController : BaseController
    {
        public ValidationController(IPhotoContestData data)
            : base(data)
        {
        }

        public virtual JsonResult UserNameExist(string userName)
        {
            if (userName.Length < 4)
            {
                return Json("Username must be minimum 4 characters long", JsonRequestBehavior.AllowGet);
            }

            var currentUserName = Data.Users
                .All()
                .Where(x => x.UserName == userName)
                .Select(x => x.UserName)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(currentUserName))
            {
                return Json(string.Format("Username {0} is already taken", currentUserName),
                    JsonRequestBehavior.AllowGet);
            }

            return Json(true, currentUserName, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult EmailExist(string email)
        {
            if (email.Length < 4)
            {
                return Json("Invalid email", JsonRequestBehavior.AllowGet);
            }

            var existingEmail = Data.Users
                .All()
                .Where(x => x.Email == email)
                .Select(x => x.Email)
                .FirstOrDefault();

            if (existingEmail != null)
            {
                return Json(string.Format("Email {0} is already exists", existingEmail), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}