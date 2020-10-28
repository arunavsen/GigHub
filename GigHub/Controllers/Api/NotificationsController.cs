using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using GigHub.DTOs;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var user = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == user && !un.IsRead)
                .Select(un => un.Notification)
                .Include(un => un.Gig.Artist)
                .ToList();

            //var c = new MapperConfiguration( x=>x.CreateMap<ApplicationUser,UserDto>());
            
            return notifications.Select(n=>new NotificationDto()

            {
                Gig = new GigDto()
                {
                    Id = n.Gig.Id,
                    DateTime = n.Gig.DateTime,
                    Venue = n.Gig.Venue,
                    IsCanceled = n.Gig.IsCanceled,
                    Artist = new UserDto()
                    {
                        UserId = n.Gig.ArtistId,
                        Name = n.Gig.Artist.Name
                    }
                },
                OriginalDateTime = n.OriginalDateTime,
                OriginalVenue = n.OriginalVenue,
                Type = n.Type,
                DateTime = n.DateTime
            });
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();
            notifications.ForEach(n => n.Read());
            _context.SaveChanges();

            return Ok();
        }
    }
}
