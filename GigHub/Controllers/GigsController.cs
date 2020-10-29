using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GIgFormViewModel()
            {
                Genres = _context.Genres.ToList(),
                Heading = "Add a Gig"
            };
            return View(viewModel);
        }

        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home",new {query = viewModel.SearchTerm});
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var user = User.Identity.GetUserId();
            var gigs = _context.Gigs.Single(g => g.Id == id && g.ArtistId == user);

            var viewModel = new GIgFormViewModel()
            {
                Id = gigs.Id,
                Genres = _context.Genres.ToList(),
                Date = gigs.DateTime.ToString("d MMM yyyy"),
                Time = gigs.DateTime.ToString("HH:mm"),
                Venue = gigs.Venue,
                Genre = gigs.GenreId,
                Heading = "Edit a Gig"
            };
            return View("Create",viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var user = User.Identity.GetUserId();
            var myGigs = _context.Gigs
                .Where(a => a.ArtistId == user && a.DateTime > DateTime.Now && !a.IsCanceled)
                .Include(a=>a.Genre)
                .ToList();

            return View(myGigs);
        }

        [Authorize]
        public ActionResult Attend()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendences
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g=>g.Artist)
                .Include(g=>g.Genre)
                .ToList();

            var user = User.Identity.GetUserId();
            var attendences = _context.Attendences
                .Where(m => m.AttendeeId == user && m.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(m => m.GigId);

            var viewModel = new GigsViewModel()
            {
                LatestGigs = gigs,
                AutheticatedUser = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm going",
                Attendences = attendences
            };
            return View("Gigs",viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GIgFormViewModel gIgFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                gIgFormViewModel.Genres = _context.Genres.ToList();
                return View(gIgFormViewModel);
            }
            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                Venue = gIgFormViewModel.Venue,
                GenreId = gIgFormViewModel.Genre,
                DateTime = gIgFormViewModel.GetDateTime()
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GIgFormViewModel gIgFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                gIgFormViewModel.Genres = _context.Genres.ToList();
                return View("Create",gIgFormViewModel);
            }

            var user = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(a => a.Id == gIgFormViewModel.Id && a.ArtistId == user);

            gig.Venue = gIgFormViewModel.Venue;
            gig.GenreId = gIgFormViewModel.Genre;
            gig.DateTime = gIgFormViewModel.GetDateTime();

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult Modified(GIgFormViewModel gIgFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                gIgFormViewModel.Genres = _context.Genres.ToList();
                return View("Create", gIgFormViewModel);
            }

            var user = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Include(b => b.Attendences.Select(c => c.Attendee))
                .Single(a => a.Id == gIgFormViewModel.Id && a.ArtistId == user);

            gigs.Modify(gIgFormViewModel.GetDateTime(), gIgFormViewModel.Venue, gIgFormViewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult Details(int id)
        {
            var gigs = _context.Gigs
                .Include(m=>m.Artist)
                .Include(m=>m.Genre)
                .SingleOrDefault(m => m.Id == id);

            if (gigs == null)
            {
                return HttpNotFound();
            }

            var gigDetails = new GigDetailsViewModel()
            {
                Gig = gigs
            };

            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity.GetUserId();
                gigDetails.IsAttending = _context.Attendences.Any(m => m.AttendeeId == user && m.GigId == gigs.Id);
                gigDetails.IsFollowing =
                    _context.Followings.Any(m => m.FollowerId == user && m.FolloweeId == gigs.ArtistId);

            }

            return View("Details",gigDetails);
        }
    }
}