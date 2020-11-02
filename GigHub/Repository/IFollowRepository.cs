using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Repository
{
    public interface IFollowRepository
    {
        IEnumerable<Following> GetFollowingList(string user);
    }
}