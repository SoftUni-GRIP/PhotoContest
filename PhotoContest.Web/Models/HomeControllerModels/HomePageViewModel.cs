namespace PhotoContest.Web.Models.HomeControllerModels
{
    using System.Collections.Generic;
    using ContestModels.ViewModels;

    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            this.ContestBasicDetails = new List<ContestBasicDetails>();
        }

        public ICollection<ContestBasicDetails> ContestBasicDetails { get; set; }

        public string CurrentUserId { get; set; }

        public bool IsAdmin { get; set; }
    }
}