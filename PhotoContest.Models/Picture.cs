namespace PhotoContest.Models
{
    using System;
    using System.Collections.Generic;
    using Contracts;

    public class Picture : IHaveCreationDate, IEntity
    {
        private ICollection<Vote> votes;

        public Picture()
        {
            votes = new HashSet<Vote>();
        }

        public int Id { get; set; }

        public string Url { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Vote> Votes
        {
            get { return votes; }

            set { votes = value; }
        }

        public DateTime CreatedOn { get; set; }
    }
}