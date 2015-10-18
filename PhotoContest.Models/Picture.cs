namespace PhotoContest.Models
{
    using System;
    using System.Collections.Generic;

    public class Picture
    {
        private ICollection<Vote> votes;

        public Picture()
        {
            this.votes = new HashSet<Vote>();
            this.UploadedOn = DateTime.Now;
        }

        public int Id { get; set; }

        public string Url { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime UploadedOn { get; set; }

        public virtual ICollection<Vote> Votes
        {
            get
            {
                return this.votes;
            }

            set
            {
                this.votes = value;
            }
        }
    }
}
