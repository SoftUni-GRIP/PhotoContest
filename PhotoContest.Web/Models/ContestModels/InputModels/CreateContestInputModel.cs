namespace PhotoContest.Web.Models.ContestModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using PhotoContest.Models.Enums;
    using PhotoContest.Models;
    using Infrastructure.Mappings;

    public class CreateContestInputModel : IMapTo<Contest>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public ContestStatusType Status { get; set; }

        [Required]
        public int WinnersCount { get; set; }

        [Required]
        public VotingStrategyType VotingStrategyType { get; set; }

        [Required]
        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public int? MaxNumberOfParticipants { get; set; }
    }
}