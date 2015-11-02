namespace PhotoContest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Enums;
    using Contracts;

    public class Contest : IEntity, IHaveCreationDate
    {
        private ICollection<User> voters; 
        private ICollection<User> participants;
        private ICollection<Picture> pictures;
        private ICollection<Reward> rewards;
        private ICollection<User> winners;

        public Contest()
        {
            this.voters = new HashSet<User>();
            this.winners = new HashSet<User>();
            this.pictures = new HashSet<Picture>();
            this.participants = new HashSet<User>();
            this.rewards = new HashSet<Reward>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ContestStatusType Status { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        // Reward strategy
        //[Range(0, 5)]
        public int WinnersCount { get; set; }

        public VotingStrategyType VotingStrategyType { get; set; }

        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        // Deadline strategy
        public DateTime? DeadlineDate { get; set; }

        public int? MaxNumberOfParticipants { get; set; }

        public DateTime ClosedOn { get; set; }

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

        public virtual ICollection<User> Voters
        {
            get
            {
                return this.voters;
            }

            set
            {
                this.voters = value;
            }
        }
        public DateTime CreatedOn { get; set; }
    }
}