using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.Repository;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendenceRepository _attendenceRepository;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _attendenceRepository = new AttendenceRepository(_context);
        }
        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _context.Gigs
                                            .Include(u => u.Artist)
                                            .Include(u=>u.Genre)
                                            .Where(u=>u.DateTime > DateTime.Now && !u.IsCanceled);

            if (!string.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs.Where(m => m.Artist.Name.Contains(query)
                                                       || m.Genre.Name.Contains(query)
                                                       || m.Venue.Contains(query));
            }

            var user = User.Identity.GetUserId();

            var following = _context.Followings
                .Where(m => m.FollowerId == user)
                .ToList()
                .ToLookup(m=>m.FolloweeId);

            var VM = new GigsViewModel()
            {
                LatestGigs = upcomingGigs,
                AutheticatedUser = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendences = _attendenceRepository.FutureAttendences(user).ToLookup(m => m.GigId),
                Following = following
            };

            return View("Gigs",VM);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}