using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.Repository
{
    public class GigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _context.Gigs
                .Include(b => b.Attendences.Select(c => c.Attendee))
                .SingleOrDefault(a => a.Id == gigId);
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _context.Attendences
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public Gig GetGig(int id)
        {
            return _context.Gigs
                .Include(m=>m.Artist)
                .Include(m=>m.Genre)
                .Single(g => g.Id == id);
        }

        public List<Gig> GetUpcomingGigsByArtist(string user)
        {
            return _context.Gigs
                .Where(a => a.ArtistId == user && a.DateTime > DateTime.Now && !a.IsCanceled)
                .Include(a => a.Genre)
                .ToList();
        }


        public void AddGig(Gig gig)
        {
            _context.Gigs.Add(gig);
        }

        public bool GigDetailsIsFollowing(string user, Gig gigs)
        {
            return _context.Followings.Any(m => m.FollowerId == user && m.FolloweeId == gigs.ArtistId);
        }

        public bool GigDetailsIsAttending(string user, Gig gigs)
        {
            return _context.Attendences.Any(m => m.AttendeeId == user && m.GigId == gigs.Id);
        }
    }
}