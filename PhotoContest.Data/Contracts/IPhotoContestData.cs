﻿namespace PhotoContest.Data.Contracts
{
    using System.Threading.Tasks;
    using Models;

    public interface IPhotoContestData
    {
        IRepository<User> Users { get; }

        IRepository<Contest> Contests { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
