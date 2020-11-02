using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.Repository
{
    public class FollowRepository : IFollowRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Following> GetFollowingList(string user)
        {
            return _context.Followings
                .Where(m => m.FollowerId == user)
                .ToList();
        }
    }
}