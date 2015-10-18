namespace PhotoContest.Web.Models.ContestModels.ViewModels
{
    using System;
    using PhotoContest.Models.Enums;

    public class CreateContestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ContestStatusType Status { get; set; }

        public int WinnersCount { get; set; }

        public VotingStrategyType VotingStrategyType { get; set; }

        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public int? MaxNumberOfParticipants { get; set; }
    }
}