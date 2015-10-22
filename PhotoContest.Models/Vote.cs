namespace PhotoContest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Contracts;

    public class Vote : IHaveCreationDate, IEntity
    {
        public int Id { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int PictureId { get; set; }

        public virtual Picture Picture { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}