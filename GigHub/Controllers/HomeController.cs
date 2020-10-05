using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.ViewModels;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var upcomingGigs = _context.Gigs
                                            .Include(u => u.Artist)
                                            .Include(u=>u.Genre)
                                            .Where(u=>u.DateTime > DateTime.Now);

            var VM = new GigsViewModel()
            {
                LatestGigs = upcomingGigs,
                AutheticatedUser = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs"
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