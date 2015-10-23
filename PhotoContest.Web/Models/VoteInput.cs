namespace PhotoContest.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Mappings;
    using PhotoContest.Models;

    public class VoteInput : IMapFrom<Vote>
    {
        [Range(0, 5)]
        public int Rating { get; set; }

        public int PictureId { get; set; }
    }
}