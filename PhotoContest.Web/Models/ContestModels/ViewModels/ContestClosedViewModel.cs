namespace PhotoContest.Web.Models.ContestModels.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    public class ContestClosedViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<string> Winners { get; set; } 

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestClosedViewModel>()
             .ForMember(x => x.Winners, setup => setup.MapFrom(m => m.Winners.Select(w => w.UserName)));
        }
    }
}