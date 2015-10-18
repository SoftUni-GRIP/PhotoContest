﻿namespace PhotoContest.Models
{
    using System;
    using System.Collections.Generic;
    using Common.Enums;
    using Contracts;

    public class Contest : IEntity, IHaveCreationDate
    {
        private ICollection<User> participants;
        private ICollection<Picture> pictures;
        private ICollection<Reward> rewards;
        private ICollection<User> winners;

        public Contest()
        {
            winners = new HashSet<User>();
            pictures = new HashSet<Picture>();
            participants = new HashSet<User>();
            CreatedOn = DateTime.Now;
            rewards = new HashSet<Reward>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ContestStatusType Status { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        //Reward strategy
        public int WinnersCount { get; set; }

        //public decimal Price { get; set; }

        public VotingStrategyType VotingStrategyType { get; set; }

        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        //Deadline strategy
        public DateTime? DeadlineDate { get; set; }

        public int? MaxNumberOfParticipants { get; set; }

        public DateTime ClosedOn { get; set; }

        public virtual ICollection<Picture> Pictures
        {
            get { return pictures; }

            set { pictures = value; }
        }

        public virtual ICollection<User> Participants
        {
            get { return participants; }

            set { participants = value; }
        }

        public virtual ICollection<User> Winners
        {
            get { return winners; }
            set { winners = value; }
        }

        public virtual ICollection<Reward> Rewards
        {
            get { return rewards; }

            set { rewards = value; }
        }

        public DateTime CreatedOn { get; set; }
    }
}