namespace PhotoContest.Web.Models.UserModels.ViewModels
{
    using AutoMapper;
    using Common.Enums;
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    public class UserDetails : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumberConfirmed { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserDetails>();
        }
    }
}