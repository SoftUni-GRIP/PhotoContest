namespace PhotoContest.Web.Models.ContestModels.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Enums;
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    public class ContestInputModel : IMapTo<Contest>
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

        public ICollection<string> UserIds { get; set; }

        public int? MaxNumberOfParticipants { get; set; }
    }
}