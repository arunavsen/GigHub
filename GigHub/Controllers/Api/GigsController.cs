using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.API
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var user = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Include(b => b.Attendences.Select(c=>c.Attendee))
                .Single(a => a.Id == id && a.ArtistId == user);

            if (gigs.IsCanceled)
            {
                return NotFound();
            }

            gigs.Cancel();
            _context.SaveChanges();

            return Ok();
        }

        
    }
}
