using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.Repository
{
    public class AttendenceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Attendence> FutureAttendences(string user)
        {
            return _context.Attendences
                .Where(m => m.AttendeeId == user && m.Gig.DateTime > DateTime.Now)
                .ToList();
        }
    }
}

