using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    [Authorize]
    public class ArtistController : Controller
    {
        private IEnumerable<ApplicationUser> Artists { get; set; }
        private readonly ApplicationDbContext _context;

        public ArtistController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Follow()
        {
            var user = User.Identity.GetUserId();
            var Artists = _context.Followings
                .Where(a => a.FollowerId == user)
                .Select(a => a.Followee)
                .ToList();

            return View(Artists);
        }
    }
}