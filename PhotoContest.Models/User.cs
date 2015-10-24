﻿namespace PhotoContest.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Reward> rewards;
        private ICollection<Contest> wonContests; 

        public User()
        {
            this.rewards = new HashSet<Reward>();
            this.wonContests = new HashSet<Contest>();
        }

        public virtual ICollection<Contest> Contests { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Reward> Rewards
        {
            get
            {
                return this.rewards;
            }

            set
            {
                this.rewards = value;
            }
        }

        public virtual ICollection<Contest> WonContests
        {
            get
            {
                return this.wonContests;
            }

            set
            {
                this.wonContests = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}