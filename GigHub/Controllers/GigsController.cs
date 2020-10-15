using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
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
            var viewModel = new GigsViewModel()
            {
                LatestGigs = gigs,
                AutheticatedUser = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm going"
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
    }
}