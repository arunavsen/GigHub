using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Repository
{
    public interface IGigRepository
    {
        Gig GetGigWithAttendees(int gigId);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        Gig GetGig(int id);
        IEnumerable<Gig> GetUpcomingGigsByArtist(string user);
        void AddGig(Gig gig);
        bool GigDetailsIsFollowing(string user, Gig gigs);
        bool GigDetailsIsAttending(string user, Gig gigs);
    }
}