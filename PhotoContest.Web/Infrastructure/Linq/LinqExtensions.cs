namespace PhotoContest.Web.Infrastructure.Linq
{
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using PhotoContest.Models;

    public static class LinqExtensions
    {
        public static IEnumerable<Contest> WhereUserIsTheContestOwner(this IEnumerable<Contest> source)
        {
            return source.Where(c => c.OwnerId == HttpContext.Current.User.Identity.GetUserId());
        }

        public static IEnumerable<Contest> WhereUserIsParticipant(this IEnumerable<Contest> source)
        {
            return source.Where(c => c.Participants.Any(p => p.Id == HttpContext.Current.User.Identity.GetUserId()) || c.Pictures.Any(pic => pic.User.Id == HttpContext.Current.User.Identity.GetUserId()));
        }

        public static IEnumerable<Contest> WhereUserIsVoter(this IEnumerable<Contest> source)
        {
            return source.Where(c => c.Voters.Any(v => v.Id == HttpContext.Current.User.Identity.GetUserId()) || c.Pictures.Any(pic => pic.Votes.Any(vot => vot.UserId == HttpContext.Current.User.Identity.GetUserId())));
        }

        public static IEnumerable<User> SelectWinners(this IEnumerable<Picture> source, int winnersCount)
        {
            return source.OrderByDescending(p => p.Votes.Average(v => v.Rating)).Select(p => p.User).Take(winnersCount);
        }

        public static IQueryable<T> WhereUsernameStartsWith<T>(this IQueryable<T> source, string input) where T : User
        {
            return source.Where(user => user.UserName.Contains(input));
        }
    }
}