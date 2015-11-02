namespace PhotoContest.Web.Infrastructure.Linq
{
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Data.Contracts;

    public static class LinqExtensions
    {
        public static IEnumerable<PhotoContest.Models.Contest> WhereUserIsTheContestOwner(this IEnumerable<PhotoContest.Models.Contest> source)
        {
            source = source.Where(c => c.OwnerId == HttpContext.Current.User.Identity.GetUserId());
            return source;
        }

        public static IEnumerable<PhotoContest.Models.Contest> WhereUserIsParticipant(this IEnumerable<PhotoContest.Models.Contest> source)
        {
            source = source.Where(c => c.Participants.Any(p => p.Id == HttpContext.Current.User.Identity.GetUserId()) || c.Pictures.Any(pic => pic.User.Id == HttpContext.Current.User.Identity.GetUserId()));
            
            return source;
        }

        public static IEnumerable<PhotoContest.Models.Contest> WhereUserIsVoter(this IEnumerable<PhotoContest.Models.Contest> source)
        {
            source = source.Where(c => c.Voters.Any(v => v.Id == HttpContext.Current.User.Identity.GetUserId())|| c.Pictures.Any(pic => pic.Votes.Any(vot => vot.UserId == HttpContext.Current.User.Identity.GetUserId())));

            return source;
        }
    }
}