using System;
using System.Collections.Generic;
using PhotoContest.Models.Contracts;
using PhotoContest.Models.Enums;

namespace PhotoContest.Models
{
    public class Contest : IEntity
    {
        private ICollection<User> winners;
        private ICollection<Picture> pictures;
        private ICollection<User> participants;

        public Contest()
        {
            this.winners = new HashSet<User>();
            this.pictures = new HashSet<Picture>();
            this.participants = new HashSet<User>();
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ContestStatusType Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        //Reward strategy
        public int WinnersCount { get; set; }

        public decimal Price { get; set; }

        public VotingStrategyType VotingStrategyType { get; set; }

        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        //Deadline strategy
        public DateTime? DeadlineDate { get; set; }

        public int? MaxNumberOfParticipants { get; set; }

        public virtual ICollection<Picture> Pictures
        {
            get
            {
                return this.pictures;
            }

            set
            {
                this.pictures = value;
            }
        }

        public virtual ICollection<User> Participants
        {
            get
            {
                return this.participants;
            }

            set
            {
                this.participants = value;
            }
        }

        public virtual ICollection<User> Winners
        {
            get
            {
                return this.winners;
            }
            set
            {
                this.winners = value;
            }
        }
    }
}
