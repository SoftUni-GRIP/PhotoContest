namespace PhotoContest.Web.Infrastructure.CacheService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Data.Contracts;
    using PhotoContest.Models;

    public class ContestCacheService : BaseCacheService, ICacheService
    {
        private readonly IPhotoContestData data;

        public ContestCacheService(IPhotoContestData data)
        {
            this.data = data;
        }

        public IList<Contest> Contests
        {
            get
            {
                return Get<IList<Contest>>("ContestBasicDetails",
                    () => data.Contests
                .All()
                .ToList());
            }
        }

        public void RemoveContestsFromCache()
        {
            HttpRuntime.Cache.Remove("ContestBasicDetails");
        }
    }
}