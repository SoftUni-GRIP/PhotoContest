namespace PhotoContest.Models
{
    public class Reward
    {
        public int Id { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public decimal RewardPrice { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}