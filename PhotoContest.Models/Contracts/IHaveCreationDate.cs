namespace PhotoContest.Models.Contracts
{
    using System;

    public interface IHaveCreationDate
    {
        DateTime CreatedOn { get; set; }
    }
}