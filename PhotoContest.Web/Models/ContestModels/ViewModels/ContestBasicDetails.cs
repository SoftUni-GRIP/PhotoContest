namespace PhotoContest.Web.Models.ContestModels.ViewModels
{
    using AutoMapper;
    using Common.Enums;
    using Infrastructure.Mappings;
    using Microsoft.AspNet.Identity;
    using PhotoContest.Models;

    public class ContestBasicDetails : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ContestStatusType Status { get; set; }

        public string Author { get; set; }


        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestBasicDetails>()
              .ForMember(x => x.Author, setup => setup.MapFrom(m => m.Owner.UserName));
        }
    }
}