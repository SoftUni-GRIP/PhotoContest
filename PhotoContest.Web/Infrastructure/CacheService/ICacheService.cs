namespace PhotoContest.Web.Infrastructure.CacheService
{
    using System.Collections.Generic;
    using PhotoContest.Models;

    public interface ICacheService
    {
        IList<Contest> Contests { get; }

        void RemoveContestsFromCache();
    }
}