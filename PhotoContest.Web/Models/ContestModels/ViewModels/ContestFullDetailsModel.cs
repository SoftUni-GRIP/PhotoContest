namespace PhotoContest.Web.Models.ContestModels.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Common.Enums;
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    public class ContestFullDetailsModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ContestStatusType Status { get; set; }

        public string Owner { get; set; }

        public VotingStrategyType VotingStrategyType { get; set; }

        public ParticipationStrategyType ParticipationStrategyType { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public int? MaxNumberOfParticipants { get; set; }

        public bool CanEdit { get; set; }

        public bool CanParticipate { get; set; }

        public bool CanVote { get; set; }

        public ICollection<PictureViewModel> Pictures { get; set; } 

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestFullDetailsModel>()
             .ForMember(x => x.Owner, setup => setup.MapFrom(m => m.Owner.UserName));
        }
    }
}