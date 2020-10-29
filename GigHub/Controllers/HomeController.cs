using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
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
            var attendences = _context.Attendences
                .Where(m => m.AttendeeId == user && m.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(m => m.GigId);

            var VM = new GigsViewModel()
            {
                LatestGigs = upcomingGigs,
                AutheticatedUser = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendences = attendences
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