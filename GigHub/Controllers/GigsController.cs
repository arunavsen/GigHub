using System;
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
                Genres = _context.Genres.ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
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

            return RedirectToAction("Index", "Home");
        }
    }
}