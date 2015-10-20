namespace PhotoContest.Web.Models.AccountModels
{
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    public class UserSearchViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}