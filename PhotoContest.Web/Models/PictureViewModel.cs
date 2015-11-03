namespace PhotoContest.Web.Models
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using AutoMapper;
    using ContestModels.ViewModels;
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    public class PictureViewModel : IMapFrom<Picture>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Author { get; set; }

        public decimal Rating { get; set; }

        public decimal DisplayRating { get; set; }

        public bool CanEdit { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Picture, PictureViewModel>()
                .ForMember(x => x.Author, setup => setup.MapFrom(m => m.User.UserName));

            //configuration.CreateMap<Picture, PictureViewModel>()
            //   .ForMember(x => x.Rating, setup => 
            //       setup.MapFrom(m => 
            //           m.Votes.Select(x => x.Rating).Average()));

            //configuration.CreateMap<Picture, PictureViewModel>()
            // .ForMember(x => x.DisplayRating, setup => setup.MapFrom(m => m.Votes.Select(x => x.Rating).Average() * 19.8));


            configuration.CreateMap<Picture, PictureViewModel>()
               .ForMember(x => x.Rating, setup =>
                   setup.MapFrom(m =>
                       !m.Votes.Any() ? 0 : m.Votes.Select(x => x.Rating).Average()
                ));

            configuration.CreateMap<Picture, PictureViewModel>()
             .ForMember(x => x.DisplayRating, setup =>
                     setup.MapFrom(m =>
                       !m.Votes.Any() ? 0 : m.Votes.Select(x => x.Rating).Average() * 19.8
                 ));
        }
    }
}