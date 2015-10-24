namespace PhotoContest.Web.Models.ContestModels.ViewModels
{
    using System.Web.Mvc;
    using AutoMapper;
    using Common.Enums;
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    
    public class ContestBasicDetails : IMapFrom<Contest>, IHaveCustomMappings
    {
        [AllowHtml]
        public int Id { get; set; }
        [AllowHtml]
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [AllowHtml]
        public ContestStatusType Status { get; set; }
        [AllowHtml]
        public string Author { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestBasicDetails>()
              .ForMember(x => x.Author, setup => setup.MapFrom(m => m.Owner.UserName));
        }
    }
}