using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using GigHub.Models;
using GigHub.Persistance;
using GigHub.Repository;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GIgFormViewModel()
            {
                Genres = _unitOfWork.Genre.GetGenres(),
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
            var gigs = _unitOfWork.Gigs.GetGig(id);

            if (gigs == null)
            {
                return HttpNotFound();
            }

            if (gigs.ArtistId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new GIgFormViewModel()
            {
                Id = gigs.Id,
                Genres = new ApplicationDbContext().Genres.ToList(),
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
            var myGigs = _unitOfWork.Gigs.GetUpcomingGigsByArtist(User.Identity.GetUserId());

            return View(myGigs);
        }

        [Authorize]
        public ActionResult Attend()
        {
            var user = User.Identity.GetUserId();

            var viewModel = new GigsViewModel()
            {
                LatestGigs = _unitOfWork.Gigs.GetGigsUserAttending(user),
                AutheticatedUser = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm going",
                Attendences = _unitOfWork.Attendence.FutureAttendences(user).ToLookup(m => m.GigId),
                Following = _unitOfWork.Follow.GetFollowingList(user).ToLookup(m => m.FolloweeId)
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
                gIgFormViewModel.Genres = _unitOfWork.Genre.GetGenres();
                return View(gIgFormViewModel);
            }
            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                Venue = gIgFormViewModel.Venue,
                GenreId = gIgFormViewModel.Genre,
                DateTime = gIgFormViewModel.GetDateTime()
            };

            _unitOfWork.Gigs.AddGig(gig);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GIgFormViewModel gIgFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                gIgFormViewModel.Genres = new ApplicationDbContext().Genres.ToList();
                return View("Create",gIgFormViewModel);
            }

            var user = User.Identity.GetUserId();
            var gig = _unitOfWork.Gigs.GetGig(gIgFormViewModel.Id);
            if (gig == null)
            {
                return HttpNotFound();
            }

            if (gig.ArtistId != user)
            {
                return new HttpUnauthorizedResult();
            }

            gig.Venue = gIgFormViewModel.Venue;
            gig.GenreId = gIgFormViewModel.Genre;
            gig.DateTime = gIgFormViewModel.GetDateTime();

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult Modified(GIgFormViewModel gIgFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                gIgFormViewModel.Genres = _unitOfWork.Genre.GetGenres();
                return View("Create", gIgFormViewModel);
            }

            var gigs = _unitOfWork.Gigs.GetGigWithAttendees(gIgFormViewModel.Id);

            if (gigs == null)
            {
                return HttpNotFound();
            }

            if (gigs.ArtistId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }

            gigs.Modify(gIgFormViewModel.GetDateTime(), gIgFormViewModel.Venue, gIgFormViewModel.Genre);

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult Details(int id)
        {
            var context = new ApplicationDbContext();
            var gigs = context.Gigs
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
                gigDetails.IsAttending = context.Attendences.Any(m => m.AttendeeId == user && m.GigId == gigs.Id);
                gigDetails.IsFollowing =
                    context.Followings.Any(m => m.FollowerId == user && m.FolloweeId == gigs.ArtistId);

            }

            return View("Details",gigDetails);
        }
    }
}