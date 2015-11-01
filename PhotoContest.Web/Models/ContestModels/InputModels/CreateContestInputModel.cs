namespace PhotoContest.Web.Models.ContestModels.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Enums;
    using Infrastructure.Mappings;
    using Infrastructure.ValidationAttrbutes;
    using PhotoContest.Models;

    public class ContestInputModel : IMapTo<Contest>
    {
        public ContestInputModel()
        {
            this.UserIds = new List<string>();
            this.Prizes = new List<decimal>();
            this.VotersIds = new List<string>();
        }

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

        [Required]
        [EnsureOneElement(ErrorMessage = "You have to create at least one prize")]
        public ICollection<decimal> Prizes { get; set; }

        public ICollection<string> VotersIds { get; set; } 

        public int? MaxNumberOfParticipants { get; set; }
    }
}